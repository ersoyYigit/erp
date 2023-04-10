class MudExColorHelper {
    
    static argbToHex (color) {
        return '#' + ('000000' + (color & 0xFFFFFF).toString(16)).slice(-6);
    }

    static hexToArgb (hexColor) {
        return parseInt(hexColor.replace('#', 'FF'), 16) << 32;
    }

    static hexToHsl(hex) {
        return this.rgbToHsl(this.hexToRgb(hex));
    }

    static hslToHex(hsl) {
        return this.rgbToHex(this.hslToRgb(hsl));
    }

    static setLuminance (hex, luminance) {
        if (!hex) {
            return hex;
        }
        var hsl = this.rgbToHsl(this.hexToRgb(hex));
        hsl.l = Math.max(Math.min(1, luminance), 0);
        return this.rgbToHex(this.hslToRgb(hsl));
    }

    static hexToRgb (hex) {
        const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);

        return result ? {
            r: parseInt(result[1], 16),
            g: parseInt(result[2], 16),
            b: parseInt(result[3], 16)
        } : null;
    }

    static rgbIntToHex (rgb) {
        let hex = Number(rgb).toString(16);

        if (hex.length < 6) {
            hex = '000000'.substring(hex.length) + hex;
        }
        return hex;
    }

    static hslToRgb (hsl) {
        const h = hsl.h,
            s = hsl.s,
            l = hsl.l;
        let hue2rgb,
            r,
            g,
            b,
            p,
            q;

        if (s === 0) {
            r = g = b = l;
        } else {
            hue2rgb = function (p, q, t) {
                if (t < 0) {
                    t += 1;
                }
                if (t > 1) {
                    t -= 1;
                }
                if (t < 1 / 6) {
                    return p + (q - p) * 6 * t;
                }
                if (t < 1 / 2) {
                    return q;
                }
                if (t < 2 / 3) {
                    return p + (q - p) * (2 / 3 - t) * 6;
                }
                return p;
            };

            q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            p = 2 * l - q;
            r = hue2rgb(p, q, h + 1 / 3);
            g = hue2rgb(p, q, h);
            b = hue2rgb(p, q, h - 1 / 3);
        }

        return {
            r: MudExNumber.constrain(Math.round(r * 255), 0, 255),
            g: MudExNumber.constrain(Math.round(g * 255), 0, 255),
            b: MudExNumber.constrain(Math.round(b * 255), 0, 255)
        };
    }

    static rgbToHsl (rgb) {
        if (!rgb) {
            return rgb;
        }

        const r = rgb.r / 255,
            g = rgb.g / 255,
            b = rgb.b / 255,
            max = Math.max(r, g, b),
            min = Math.min(r, g, b),
            l = (max + min) / 2;
        let h,
            s,
            d;

        if (max === min) {
            h = s = 0;
        } else {
            d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            switch (max) {
            case r:
                h = (g - b) / d + (g < b ? 6 : 0);
                break;
            case g:
                h = (b - r) / d + 2;
                break;
            case b:
                h = (r - g) / d + 4;
                break;
            }
            h /= 6;
        }

        return { h: h, s: s, l: l };
    }

    static rgbToHex (rgb) {
        const tohex = function (value) {
            const hex = value.toString(16);

            return hex.length === 1 ? '0' + hex : hex;
        };

        return '#' + tohex(rgb.r) + tohex(rgb.g) + tohex(rgb.b);
    }

    static getOptimalForegroundColor (backgroundColor, lightforegroundResult, darkforegroundResult) {
        if (this.perceivedBrightness(backgroundColor) > 190) {
            return darkforegroundResult || '#000000';
        }
        return lightforegroundResult || '#FFFFFF';
    }

    static perceivedBrightness (color) {
        if (!color) {
            return 0;
        }
        if (typeof color !== 'object') {
            color = this.hexToRgb(color);
        }
        return Math.sqrt(color.r * color.r * 0.241 + color.g * color.g * 0.691 + color.b * color.b * 0.068);
    }
    
}
window.MudExColorHelper = MudExColorHelper;
class MudExDomHelper {
    dom;
    
    constructor(dom) {
        this.dom = dom;
    }

    _asArray(val) {
        return Array.isArray(val) ? val : [val];
    }

    down(q) {
        var d = this.dom.querySelector(q);
        return d ? new MudExDomHelper(d) : null;
    }

    isVisible(deep) {
        if (!deep) {
            return this.dom.style.display !== 'none' && this.dom.style.visibility !== 'hidden';
        }
        var element = this.dom;
        if (element.offsetWidth === 0 || element.offsetHeight === 0) return false;
        var height = document.documentElement.clientHeight,
            rects = element.getClientRects(),
            onTop = function (r) {
                var x = (r.left + r.right) / 2, y = (r.top + r.bottom) / 2;
                return document.elementFromPoint(x, y) === element;
            };
        for (var i = 0, l = rects.length; i < l; i++) {
            var r = rects[i],
                inViewport = r.top > 0 ? r.top <= height : (r.bottom > 0 && r.bottom <= height);
            if (inViewport && onTop(r)) return true;
        }
        return false;
    }

    setStyle(styles) {
        Object.assign(this.dom.style, styles);
        return this;
    }
    
    getStyleValue(name) {
        return this.dom.style.getPropertyValue(name);
    }

    addCls(names) {
        this._asArray(names).forEach((css) => this.dom.classList.add(css));
        return this;
    }

    removeCls(names) {
        this._asArray(names).forEach((css) => this.dom.classList.remove(css));
        return this;
    }

    toggleCls(names) {
        this._asArray(names).forEach((css) => {
            if (this.dom.classList.contains(css)) {
                this.removeCls(css);
            } else {
                this.addCls(css);
            }
        });
        return this;
    }

    show() {
        this.dom.style.display = '';
        return this;
    }

    hide(animate) {
        this.dom.style.display = 'none';
        return this;
    }

    getBounds() { return this.dom.getBoundingClientRect(); }
    getWidth() { return this.getBounds().width; }
    getHeight() { return this.getBounds().height; }
    getX() { return this.getXY()[0]; }
    getY() { return this.getXY()[1]; }
    getXY() {
        var round = Math.round,
            body = document.body,
            dom = this.dom,
            x = 0,
            y = 0,
            bodyRect, rect;

        if (dom !== document && dom !== body) {
            // IE (including IE10) throws an error when getBoundingClientRect
            // is called on an element not attached to dom
            try {
                bodyRect = body.getBoundingClientRect();
                rect = dom.getBoundingClientRect();

                x = rect.left - bodyRect.left;
                y = rect.top - bodyRect.top;
            }
            catch (ex) {
                // This block is intentionally left blank
            }
        }
        return [round(x), round(y)];
    }
}

window.MudExDomHelper = MudExDomHelper;
class MudExEventHelper {
    static isWithin(event, element) {
        if (!element || !event) {
            return false;
        }
        let rect = element.getBoundingClientRect();
        return (event.clientX > rect.left &&
        event.clientX < rect.right &&
        event.clientY < rect.bottom &&
        event.clientY > rect.top);
    }
}

window.MudExEventHelper = MudExEventHelper;
class MudExNumber {
    static constrain (number, min, max) {
        var x = parseFloat(number);
        if (min === null) {
            min = number;
        }

        if (max === null) {
            max = number;
        }
        return (x < min) ? min : ((x > max) ? max : x);
    }
}

window.MudExNumber = MudExNumber;
class MudBlazorExtensionHelper {
    mudDialogSelector;
    mudDialogHeaderSelector;
    onDone;
    observer;
    options;
    dialog;
    dotnet;

    constructor(options, dotNet, onDone) {
        this.dotnet = dotNet;
        this.onDone = onDone;
        this.options = options;
        this.mudDialogSelector = options.mudDialogSelector || '.mud-dialog:not([data-mud-extended=true])';
        this.mudDialogHeaderSelector = options.mudDialogHeaderSelector || '.mud-dialog-title';
        this.dialog = document.querySelector(this.mudDialogSelector);
        this.dialogHeader = this.dialog.querySelector(this.mudDialogHeaderSelector);
    }

    findDialog() {
        return Array.from(document.querySelectorAll(this.mudDialogSelector)).filter(d => !d.__extended)[0];
    }

    init() {
        this.dialog = this.dialog || this.findDialog();
        if (!this.dialog) {
            this.observer = new MutationObserver((e) => {
                var res = e.filter(x => x.addedNodes && x.addedNodes[0] === this.findDialog());
                if (res.length) {
                    this.dialog = res[0].addedNodes[0];
                    this.observer.disconnect();
                    this.handleExtensions();
                }
            });
            this.observer.observe(document, { characterData: true, childList: true, subtree: true });
        } else {
            this.handleExtensions();
        }
    }

    handleExtensions() {
        this.dialog.__extended = true;
        this.dialog.setAttribute('data-mud-extended', true);
        if (this.options.showAtCursor) {
            this.moveElementToMousePosition(this.dialog);
        }
        setTimeout(() => this.dialog.classList.remove('mud-ex-dialog-initial'), 50);
        // For animations
        if (this.options.animations != null && Array.isArray(this.options.animations) && this.options.animations.length) {
            this.animate(this.options.animationDescriptions);
        }

        if (this.options.disablePositionMargin) {
            this.dialog.classList.add('mud-dialog-position-fixed');
        }

        // Full height ext
        if (this.options.fullHeight) {
            var cls = this.options.disableSizeMarginY ? 'mud-dialog-height-full-no-margin' : 'mud-dialog-height-full';
            this.dialog.classList.add(cls);
            var actions = this.dialog.querySelector('.mud-dialog-actions');
            if (actions) {
                actions.classList.add('mud-dialog-actions-fixed-bottom');
            }
        }
        if (this.options.fullWidth && this.options.disableSizeMarginX) {
            this.dialog.classList.remove('mud-dialog-width-full');
            this.dialog.classList.add('mud-dialog-width-full-no-margin');
            if (this.dialog.classList.contains('mud-dialog-width-false')) {
                this.dialog.classList.remove('mud-dialog-width-false');
            }
        }

        // Inject buttons
        if (this.options.buttons && this.options.buttons.length) {
            var dialogButtonWrapper = document.createElement('div');
            dialogButtonWrapper.classList.add('mud-ex-dialog-header-actions');
            if (!this.options.closeButton) {
                dialogButtonWrapper.style.right = '8px'; // No close button, so we need to move the buttons to the right (48 - button width of 40)
            }
            
            if (this.dialogHeader) {
                dialogButtonWrapper = this.dialogHeader.insertAdjacentElement('beforeend', dialogButtonWrapper);
            }
            this.options.buttons.reverse().forEach(b => {
                if (dialogButtonWrapper) {
                    dialogButtonWrapper.insertAdjacentHTML('beforeend', b.html);
                    var btnEl = dialogButtonWrapper.querySelector('#' + b.id);
                    btnEl.onclick = () => {
                        if (b.id.indexOf('mud-button-maximize') >= 0) {
                            this.maximize();
                        }
                        if (b.id.indexOf('mud-button-minimize') >= 0) {
                            this.minimize();
                        } else {
                            b.callBackReference.invokeMethodAsync(b.callbackName);
                        }
                    }
                }
            });
        }

        
        if (this.options.modal === false) {
            this.dialog.parentElement.querySelector('.mud-overlay').style.display = 'none';
            setTimeout(() => {
                this.makeDialogAbsolute();
                this.dialog.parentElement.style.height = '0';
            }, this.options.animationDurationInMs + 50);
        }

        // Handle drag
        if (this.options.dragMode !== 0 && this.dialog) {
            this.dragElement(this.dialog, this.dialogHeader, document.body, this.options.dragMode === 2);
            this.done();
        }
        
        // Resize
        this.checkResizeable();
    }

    minimize() {
        let targetElement = document.querySelector(`.mud-ex-task-bar-item-for-${this.dialog.id}`);
        this.moveToElement(targetElement);
    }

    moveToElement(targetElement) {
        var rect = targetElement.getBoundingClientRect();
        this.dialog.style.height = targetElement.style.height;
        this.dialog.style.width = targetElement.style.width;
        this.dialog.style.left = rect.left + "px";
        this.dialog.style.top = rect.top + "px";
    }

    makeDialogAbsolute() {
        this.dialog.style.position = 'absolute';
        var rect = this.dialog.getBoundingClientRect();
        this.dialog.style.left = rect.left + "px";
        this.dialog.style.top = rect.top + "px";
    }

    moveElementToMousePosition(element) {
        var e = MudBlazorExtensions.getCurrentMousePosition();
        var x = e.clientX;
        var y = e.clientY;
        var origin = this.options.cursorPositionOriginName.split('-');
        
        var maxWidthFalseOrLargest = this.options.maxWidth === 6 || this.options.maxWidth === 4; // 4=xxl 6=false
        if (!this.options.fullWidth || !maxWidthFalseOrLargest) {
            if (origin[1] === 'left') {
                element.style.left = x + 'px';
            } else if (origin[1] === 'right') {
                element.style.left = (x - element.offsetWidth) + 'px';
            } else if (origin[1] === 'center') {
                element.style.left = (x - element.offsetWidth / 2) + 'px';
            }
        }
        if (!this.options.fullHeight) {
            if (origin[0] === 'top') {
                element.style.top = y + 'px';
            } else if (origin[0] === 'bottom') {
                element.style.top = (y - element.offsetHeight) + 'px';
            } else if (origin[0] === 'center') {
                element.style.top = (y - element.offsetHeight / 2) + 'px';
            }
        }
        this.ensureElementIsInScreenBounds(element);
    }

    ensureElementIsInScreenBounds(element) {
        var rect = element.getBoundingClientRect();
        var rectIsEmpty = rect.width === 0 && rect.height === 0;
        if (rectIsEmpty) {
            const ro = new ResizeObserver(entries => {
                ro.disconnect();
                this.ensureElementIsInScreenBounds(element);
            });

            ro.observe(element);
            return;
        }
        
        var animationIsRunning = !!element.getAnimations().length;
        if (animationIsRunning) {
            element.addEventListener('animationend', (e) => this.ensureElementIsInScreenBounds(element), { once: true});
            return;
        }
        if (rect.left < 0) {
            element.style.left = '0px';
        }
        if (rect.top < 0) {
            element.style.top = '0px';
        }
        if (rect.right > window.innerWidth) {
            element.style.left = (window.innerWidth - element.offsetWidth) + 'px';
        }
        if (rect.bottom > window.innerHeight) {
            element.style.top = (window.innerHeight - element.offsetHeight) + 'px';
        }
    }

    checkResizeable() {
        this.dialog.style.position = 'absolute';
        if (this.options.resizeable) {
            //this.dialog.style.position = 'absolute';
            this.dialog.style['resize'] = 'both';
            this.dialog.style['overflow'] = 'auto';
            this.dialog.style.maxWidth = window.innerWidth + 'px';
            this.dialog.style.maxHeight = window.innerHeight + 'px';
            this.dialog.style.minWidth = '100px';
            this.dialog.style.minHeight = '100px';
        }
    }

    maximize() {
        if (this._oldStyle) {
            this.dialog.style = this._oldStyle;
            delete this._oldStyle;
        } else {
            this._oldStyle = this.dialog.style;
            this.dialog.style.position = 'absolute';
            this.dialog.style.left = "0";
            this.dialog.style.top = "0";

            this.dialog.style.maxWidth = this.dialog.style.width = window.innerWidth + 'px';
            this.dialog.style.maxHeight = this.dialog.style.height = window.innerHeight + 'px';
        }
        this.checkResizeable();
    }

    done() {
        if (this.observer)
            this.observer.disconnect();
        if (this.onDone)
            this.onDone();
    }

    dragElement(dialogEl, headerEl, container, disableBoundCheck) {
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
        container = container || document.body;
        if (headerEl) {
            // if present, the header is where you move the DIV from:
            headerEl.style.cursor = 'move';
            headerEl.onmousedown = dragMouseDown;
        } else {
            // otherwise, move the DIV from anywhere inside the DIV:
            dialogEl.onmousedown = dragMouseDown;
        }

        function dragMouseDown(e) {
            e = e || window.event;
            e.preventDefault();
            // get the mouse cursor position at startup:
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.onmouseup = closeDragElement;
            // call a function whenever the cursor moves:
            document.onmousemove = elementDrag;
        }

        function elementDrag(e) {
            e = e || window.event;
            e.preventDefault();
            // calculate the new cursor position:

            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            var bounds = {
                x: container.offsetWidth - dialogEl.offsetWidth,
                y: (container === document.body ? window.innerHeight : container.offsetHeight) - dialogEl.offsetHeight
            }

            var aY = (dialogEl.offsetTop - pos2);
            var aX = (dialogEl.offsetLeft - pos1);
            dialogEl.style.position = 'absolute';

            if (disableBoundCheck || (aX > 0) && (aX < bounds.x)) {
                dialogEl.style.left = (aX) + "px";
            }
            if (disableBoundCheck || (aY > 0) && (aY < bounds.y)) {
                dialogEl.style.top = (aY) + "px";
            }


            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;

        }

        function closeDragElement() {
            // stop moving when mouse button is released:
            document.onmouseup = null;
            document.onmousemove = null;
        }
    }

    animate(types) {
        //var names = types.map(type => this.options.dialogPositionNames.map(n => `kf-mud-dialog-${type}-${n} ${this.options.animationDurationInMs}ms ${this.options.animationTimingFunctionString} 1 alternate`));
        //this.dialog.style.animation = `${names.join(',')}`;
        this.dialog.style.animation = `${this.options.animationStyle}`;
    }
}

window.MudBlazorExtensionHelper = MudBlazorExtensionHelper;


window.MudBlazorExtensions = {
    helper: null,
    currentMouseArgs: null,
    
    __bindEvents: function () {
        var onMouseUpdate = function(e) {
            window.MudBlazorExtensions.currentMouseArgs = e;
        }
        document.addEventListener('mousemove', onMouseUpdate, false);
        document.addEventListener('mouseenter', onMouseUpdate, false);
    },
    
    getCurrentMousePosition: function() {
        return window.MudBlazorExtensions.currentMouseArgs;
    },

    setNextDialogOptions: function (options, dotNet) {
        new MudBlazorExtensionHelper(options, dotNet, () => {
            MudBlazorExtensions.helper = null;
            delete MudBlazorExtensions.helper;
        }).init();
    },

    addCss: function (cssContent) {
        var css = cssContent,
            head = document.head || document.getElementsByTagName('head')[0],
            style = document.createElement('style');

        head.appendChild(style);

        style.type = 'text/css';
        if (style.styleSheet) {
            // This is required for IE8 and below.
            style.styleSheet.cssText = css;
        } else {
            style.appendChild(document.createTextNode(css));
        }
    },

    downloadFile(options) {
        var fileUrl = options.url || "data:" + options.mimeType + ";base64," + options.base64String;
        fetch(fileUrl)
            .then(response => response.blob())
            .then(blob => {
                var link = window.document.createElement("a");
                //link.href = window.URL.createObjectURL(blob, { type: options.mimeType });
                link.href = window.URL.createObjectURL(blob);
                link.download = options.fileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            });
    },

    attachDialog(dialogId) {
        if (dialogId) {
            let dialog = document.getElementById(dialogId);
            if (dialog) {
                let titleCmp = dialog.querySelector('.mud-dialog-title');
                let iconCmp = titleCmp ? titleCmp.querySelector('svg') : null;
                const res = {
                    title: titleCmp ? titleCmp.innerText : 'Unnamed window',
                    icon: iconCmp ? iconCmp.innerHTML : ''
                }
                return res;
            }
        }
        return null;
    }

};

window.MudBlazorExtensions.__bindEvents();