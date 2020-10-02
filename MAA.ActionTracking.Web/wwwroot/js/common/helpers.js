let Helpers = {
    Utility: {
        GUID: function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c === 'x' ? r : r & 0x3 | 0x8;
                return v.toString(16);
            });
        },
        HasValue: function (value) {

            if (value === undefined) return false;
            if (value === null) return false;
            if (value === "") return false;

            return true;

        },
        IsValidEmail: function (sEmail) {
            if (sEmail.length === 0) return true;

            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) {
                return true;
            } else {
                return false;
            }
        }
    },
    Parse: {

        BoolToBinaryIfRequired: function (value) {
            if (value && (value.toLowerCase() === 'true' || value.toLowerCase() === 'false')) {
                value = value.toLowerCase() === 'true' ? '1' : '0';
            }
            else if (value && (value.toLowerCase() === 'yes' || value.toLowerCase() === 'no')) {
                value = value.toLowerCase() === 'yes' ? '1' : '0';
            }
            return value;
        },
        NaNToZero: function (value) {
            if ($.isNumeric(value)) {
                return parseFloat(value);
            }
            return 0;
        },

        NullToString: function (value) {
            if (value === undefined || value === null)
                return '';
            else
                return value;
        },
        ToInt: function (value) {
            return parseInt(value);
        },
        TryNumeric: function (value) {
            //Return number if it can if not return object
            if ($.isNumeric(value)) {
                return parseFloat(value);
            }

            return value;
        },

        ToPascal: (s) => {

            return s.charAt(0).toUpperCase() + s.substr(1);
        },

        IsObject : function (o) {
            return o === Object(o) && !$.isArray(o) && typeof o !== 'function';
        },

        KeysToPascal : function (o) {
            if (Helpers.Parse.IsObject(o)) {
                const n = {};

                Object.keys(o)
                    .forEach((k) => {
                        n[Helpers.Parse.ToPascal(k)] = Helpers.Parse.KeysToPascal(o[k]);
                    });

                return n;
            } else if ($.isArray(o)) {
                return o.map((i) => {
                    return Helpers.Parse.KeysToPascal(i);
                });
            }
            return o;
        },
        PascalToWords : function (o) {
            return o.replace(/([a-z])([A-Z])/g, '$1 $2');
        }
    },
    Math: {
        Sum: function (array, round) {
            if (!$.isArray(array)) throw 'Helpers.Math.Sum: array not set or not an array.';

            var sum = 0;

            array.forEach(function (item) {
                if (!$.isNumeric(item)) throw 'Helpers.Math.Sum: (' + item + ') is not numeric.';

                sum += parseFloat(this);
            });

            if (HelpersValidate.HasValue(round)) return Helpers.Meth.Round(sum);

            return sum;

        },
        Round: function (value, decimals) {
            if (!$.isNumeric(value)) return null;
            return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
        },
        ToInt: function (value) {
            if (!$.isNumeric(value)) return null;
            return Math.ceil(value);
        },
        Product: function (a, b) {

            // get number of decimal places to shift
            exp = b.toString().length - 2;

            function makeInt(num) {
                return num * Math.pow(10, exp);
            }

            function makeFloat(num) {
                return num / Math.pow(100, exp);
            }

            return makeFloat(makeInt(a) * makeInt(b));
        }
    },
    Format: {
        Decimal: function (value, includingDigits) {
            if (!$.isNumeric(value)) return undefined;
            var result = Helpers.Math.Round(value, 2);
            if (includingDigits !== undefined && includingDigits === true)
                return result.toFixed(2);
            else
                return result;
        },
        HtmlEscapeEntities: function (d) {
            return typeof d === 'string' ?
                d.replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') :
                d;
        },
        Currency: function (d, thousands, decimal, precision, prefix, postfix) {
            d = d || 0;
            if (typeof d !== 'number' && typeof d !== 'string') {
                return d;
            }

            var negative = d < 0 ? '-' : '';
            var flo = parseFloat(d);

            // If NaN then there isn't much formatting that we can do - just
            // return immediately, escaping any HTML (this was supposed to
            // be a number after all)
            if (isNaN(flo)) {
                return Helpers.Format.HtmlEscapeEntities(d);
            }

            flo = flo.toFixed(precision);
            d = Math.abs(flo);

            var intPart = parseInt(d, 10);
            var floatPart = precision ?
                decimal + (d - intPart).toFixed(precision).substring(2) :
                '';

            return negative + (prefix || '') +
                intPart.toString().replace(
                    /\B(?=(\d{3})+(?!\d))/g, thousands
                ) +
                floatPart +
                (postfix || '');
        },
        //Currency: function (value) {
        //    if (!$.isNumeric(value)) return undefined;
        //    return "$" + Helpers.Meth.Round(value, 2).toFixed(2);
        //},
        Percent: function (value, decimals) {
            if (!$.isNumeric(value)) return undefined;
            return Helpers.Math.Round(value, 2).toFixed(decimals || 7) + "%";
        },
        DateTime: function (value, format) {
            if (!moment(value).isValid()) return undefined;
            if (!format)
                return moment(value).format("DD/MM/YYYY hh:mm A");
            else
                return moment(value).format(format);
        },
        Date: function (value, format) {
            if (!moment(value).isValid()) return undefined;
            if (!format)
                return moment(value).format("DD/MM/YYYY");
            else
                return moment(value).format(format);
        },
        Time: function (value, format) {
            if (!moment(value).isValid()) return undefined;
            if (!format)
                return moment(value).format("hh:mm A");
            else
                return moment(value).format(format);
        },

        TruncateString: function (inputString, length) {

            if (inputString === null || typeof inputString === 'undefined')
                return '';

            if (inputString.length >= length) {
                var shortText = jQuery.trim(inputString).substring(0, length)
                    .split(" ").slice(0, -1).join(" ") + " (...)";
                return shortText;
            } else return inputString;
        }
    },
    List: {
        FindByText: function (source, text) {
            // This is used to interogate simple Select Lists by text search

            for (var i = 0; i < source.length; i++) {
                if (source[i].Text.toLowerCase() === text.toLowerCase()) {
                    return source[i];
                }
            }

            //TODO: This may need to be handled.
            throw "Couldn't find object with Text: " + text;
        },
        FindByTextValue: function (source, value, notFoundText) {
            // This is used to interogate simple Select Lists by text search

            if (source instanceof Array) {
                for (var i = 0; i < source.length; i++) {
                    if (source[i].Value === value) {
                        return source[i].Text;
                    }
                }
            } else {
                if (source === value) {
                    return source;
                }
            }

            if (notFoundText === null) notFoundText = "[Not Found]";

            return notFoundText;

            //TODO: This may need to be handled.
            //throw "Couldn't find object with Text: " + text;
        }
    },
    SortByColumn: {
        setupColumn: function (element) {
            if ($(element).find('.KOSortableFlag').length === 0) {
                $(element).prepend("<i class='KOSortableFlag glyphicon glyphicon-sort disabled'></i>");
                $(element).css('cursor', 'pointer');
            }
        },
        resetSortFlags: function (element) {
            $(element).parent().find('.KOSortFlag').remove();
            $(element).parent().find('.KOSortableFlag').show();
        },
        setColumnSort: function (element, ascending) {
            $(element).find('.KOSortableFlag').hide();

            if (ascending) {
                $(element).prepend("<i class='KOSortFlag glyphicon glyphicon-sort-by-attributes'></i>");
            } else {
                $(element).prepend("<i class='KOSortFlag glyphicon glyphicon-sort-by-attributes-alt'></i>");
            }

        },
        sort: function (rec1, rec2, asc) {

            rec1 = this.parseForSort(rec1);
            rec2 = this.parseForSort(rec2);

            if (asc) {
                return rec1 === rec2 ? 0 : rec1 < rec2 ? -1 : 1;
            } else {
                return rec2 === rec1 ? 0 : rec2 < rec1 ? -1 : 1;
            }
        },
        parseForSort: function (value) {

            //CURRENCY/NUMBER
            var sortVal2 = value;
            switch (jQuery.type(value)) {
                case "boolean":
                    sortVal2 = "" + value;
                    return value;
                case "number":
                    sortVal2 = "" + value;
                    return Number(sortVal2);
                case "string":
                    sortVal2 = value.replace("$", "");
                    return value;
                case "date":
                    if (moment(value, "DD/MM/YYYY").isValid()) {
                        return moment(value, "DD/MM/YYYY h:mm:ss a");
                    }
                    return value;
                default:
                    raiseAlert(null,
                        "error",
                        "Sorting not implemented",
                        "Sorting not implemented for type: " + jQuery.type(value) + " in parseForSort.");
                    return value;
            }
        }
    },
    IsTouchDevice: function () {
        try {
            var supportsTouch = 'ontouchstart' in window || navigator.msMaxTouchPoints;
            if (supportsTouch === true) return true;
            return false;
        }
        catch (errorSupportsTouch) {
            return false;
        }
    },
    PlaySound: function (soundfile_ogg, soundfile_mp, soundfile_ma) {
        if ("Audio" in window) {
            var a = new Audio();
            if (!(a.canPlayType && a.canPlayType('audio/ogg; codecs="vorbis"')
                .replace(/no/, '')))
                a.src = soundfile_ogg;
            else if (!(a.canPlayType && a.canPlayType('audio/mpeg;').replace(/no/,
                '')))
                a.src = soundfile_mp;
            else if (!(a.canPlayType && a.canPlayType(
                'audio/mp4; codecs="mp4a.40.2"').replace(/no/, '')))
                a.src = soundfile_ma;
            else
                a.src = soundfile_mp;

            a.autoplay = true;
            return;
        } else {
            alert("Time almost up");
        }
    },
    ShowErrorMessage: function (label, message) {
            toastr.error(message, label, {
                closeButton: true
            });
    },
    ShowWarningMessage: function (label, message) {
        toastr.warning(message, label, {
            closeButton: true
        });
    },
    ShowInfoMessage: function (label, message) {
        toastr.info(message, label, {
            closeButton: true
        });
    },
    ShowSuccessMessage: function (label, message) {
        toastr.success(message, label, {
            closeButton: true
        });
    },
    RaiseEvent: function (target, name, data,allowBubble) {

        //Create & Dispatch the event
        target.dispatchEvent(new CustomEvent(name, { bubbles: allowBubble, detail: data }));
    }
    //,
    //LocalStorage: {
    //    KeyFormat: "__TENANT_ID__-__LOGGED_IN_USER_ID__-__KEY__",
    //    GetFormattedKey: function (key) {
    //        return Helpers.LocalStorage.KeyFormat
    //            .replace("__TENANT_ID__", Globals.Config.TenantId)
    //            .replace("__LOGGED_IN_USER_ID__", Globals.Config.LoggedInUserPky)
    //            .replace("__KEY__", key);
    //    },
    //    SetItemPerUserPerTenant: function (key, item) {
    //        try { window.localStorage.setItem(Helpers.LocalStorage.GetFormattedKey(key), JSON.stringify(item)); }
    //        catch (e) { console.error(e); }
    //    },
    //    GetItemPerUserPerTenant: function (key) {
    //        try { return JSON.parse(window.localStorage.getItem(Helpers.LocalStorage.GetFormattedKey(key))); }
    //        catch (e) { console.error(e); }
    //        return null;
    //    },
    //    RemoveItemPerUserPerTenant: function (key) {
    //        try { window.localStorage.removeItem(Helpers.LocalStorage.GetFormattedKey(key)); } catch (e) { }
    //    },
    //},
};


