using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Application.Enums
{
    public enum ExchangeType
    {
        ForexBuying, ForexSelling,
        BanknoteBuying, BanknoteSelling
    }

    public enum CurrencyCode
    {
        USD, AUD, DKK,
        EUR, GBP, CHF,
        SEK, CAD, KWD,
        NOK, SAR, JPY,
        BGN, RON, RUB,
        IRR, CNY, PKR,
        TRY
    }
}
