using ArdaManager.Application.Exceptions;
using ArdaManager.Application.Features.Currencies.Commands;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Domain.Entities.Misc;
using ArdaManager.Infrastructure.Contexts;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ArdaManager.Infrastructure.Services
{


    public class ExchangeService : IExchangeService
    {
        private readonly VappContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        
        private List<Currency> _dbCurrencies = new();

        public ExchangeService(VappContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _dbCurrencies = _context.Currencies.ToList();
        }




        public async Task<Result<IEnumerable<GetExchangeRatesByDateResponse>>> GetExchangeRatesByDate(DateTime? CurrencyDate)
        {
            DateTime date = (CurrencyDate.HasValue) ? CurrencyDate.Value : date = DateTime.UtcNow;
            string link = GenerateLink(date);

            string message = "Başarılı";

            try
            {
                XmlTextReader rdr = new XmlTextReader(link);
                // XmlTextReader nesnesini yaratıyoruz ve parametre olarak xml dokümanın urlsini veriyoruz
                // XmlTextReader urlsi belirtilen xml dokümanlarına hızlı ve forward-only giriş imkanı sağlar.
                XmlDocument myxml = new XmlDocument();
                // XmlDocument nesnesini yaratıyoruz.
                myxml.Load(rdr);
                // Load metodu ile xml yüklüyoruz
                XmlNode tarih = myxml.SelectSingleNode("/Tarih_Date/@Tarih");
                XmlNodeList mylist = myxml.SelectNodes("/Tarih_Date/Currency");
                XmlNodeList adi = myxml.SelectNodes("/Tarih_Date/Currency/Isim");
                XmlNodeList kod = myxml.SelectNodes("/Tarih_Date/Currency/@Kod");
                XmlNodeList doviz_alis = myxml.SelectNodes("/Tarih_Date/Currency/ForexBuying");
                XmlNodeList doviz_satis = myxml.SelectNodes("/Tarih_Date/Currency/ForexSelling");
                XmlNodeList efektif_alis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteBuying");
                XmlNodeList efektif_satis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteSelling");

                List<ExchangeRate> rates = new();

                ExchangeRate tl = new ExchangeRate()
                {
                    CurrencyId = 1,
                    RateDate = date,
                    ForexBuying = 1,
                    ForexSelling = 1,
                    BanknoteBuying = 1,
                    BanknoteSelling = 1
                };
                rates.Add(tl);

                UpsertExchangeRatesCommand tlCommand = new UpsertExchangeRatesCommand()
                {
                    CurrencyId = tl.CurrencyId,
                    BanknoteBuying = tl.BanknoteBuying,
                    BanknoteSelling = tl.BanknoteSelling,
                    ForexBuying = tl.ForexBuying,
                    ForexSelling = tl.ForexSelling,
                    RateDate = tl.RateDate
                };
                await _mediator.Send(tlCommand);
                

                foreach (Currency currency in _dbCurrencies)
                {
                    for (int i = 0; i < adi.Count; i++)
                    {
                        if (currency.Code == kod.Item(i).InnerText.ToString())
                        {
                            string fxBuyStr = doviz_alis.Item(i).InnerText.ToString();
                            fxBuyStr = fxBuyStr.Replace('.', ',');
                            decimal.TryParse( fxBuyStr, NumberStyles.AllowDecimalPoint, new CultureInfo("tr-TR"), out decimal fxBuy);
                            fxBuy = Math.Round(fxBuy, 4);


                            string fxSellStr = doviz_satis.Item(i).InnerText.ToString();
                            fxSellStr = fxSellStr.Replace('.', ',');
                            decimal.TryParse(fxSellStr, NumberStyles.AllowDecimalPoint, new CultureInfo("tr-TR"), out decimal fxSell);
                            fxSell = Math.Round(fxSell, 4);

                            string bnktBuyStr = efektif_alis.Item(i).InnerText.ToString();
                            bnktBuyStr = bnktBuyStr.Replace('.', ',');
                            decimal.TryParse(bnktBuyStr, NumberStyles.AllowDecimalPoint, new CultureInfo("tr-TR"), out decimal bnktBuy);
                            bnktBuy = Math.Round(bnktBuy, 4);


                            string bnktSellStr = efektif_satis.Item(i).InnerText.ToString();
                            bnktSellStr = bnktSellStr.Replace('.', ',');

                            decimal.TryParse(bnktSellStr, NumberStyles.AllowDecimalPoint, new CultureInfo("tr-TR"), out decimal bnktSell);
                            bnktSell = Math.Round(bnktSell, 4);

                            ExchangeRate rate = new ExchangeRate()
                            {
                                CurrencyId = currency.Id,
                                RateDate = date,


                                ForexBuying = fxBuy,
                                ForexSelling = fxSell,
                                BanknoteBuying = bnktBuy,
                                BanknoteSelling = bnktSell
                            };

                            rates.Add(rate);

                            UpsertExchangeRatesCommand command = new UpsertExchangeRatesCommand()
                            {
                                CurrencyId = rate.CurrencyId,
                                BanknoteBuying = rate.BanknoteBuying,
                                BanknoteSelling = rate.BanknoteSelling,
                                ForexBuying = rate.ForexBuying ,
                                ForexSelling = rate.ForexSelling,   
                                RateDate= rate.RateDate
                            };
                            var result = await _mediator.Send(command);
                            message = result.Messages.FirstOrDefault();
                        }
                        
                    }

                }


                var mappedExchangeRates = rates.Select(er => _mapper.Map<GetExchangeRatesByDateResponse>(er)).ToList();
                return await Result<IEnumerable<GetExchangeRatesByDateResponse>>.SuccessAsync(mappedExchangeRates,message);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<GetExchangeRatesByDateResponse>>.FailAsync(ex.Message+ "--- DB MESAJI --- " + message);
            }
        }


        public static string GenerateLink(DateTime date)
        {
            string yearStr = String.Format("{0:0000}", date.Year);
            string monthStr = String.Format("{0:00}", date.Month);
            string dayStr = String.Format("{0:00}", date.Day);

            return $"http://www.tcmb.gov.tr/kurlar/{yearStr}{monthStr}/{dayStr}{monthStr}{yearStr}.xml";
        }
    }
}
