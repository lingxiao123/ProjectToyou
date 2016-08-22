/*
    http://www.JSON.org/json2.js
    2011-02-23

    Public Domain.

    NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

    See http://www.JSON.org/js.html


    This code should be minified before deployment.
    See http://javascript.crockford.com/jsmin.html

    USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
    NOT CONTROL.


    This file creates a global JSON object containing two methods: stringify
    and parse.

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, strict: false, regexp: false */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    "use strict";

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                this.getUTCFullYear()     + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate())      + 'T' +
                f(this.getUTCHours())     + ':' +
                f(this.getUTCMinutes())   + ':' +
                f(this.getUTCSeconds())   + 'Z' : null;
        };

        String.prototype.toJSON      =
            Number.prototype.toJSON  =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"' : '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

// If the string contains no control characters, no quote characters, and no
// backslash characters, then we can safely slap some quotes around it.
// Otherwise we must also replace the offending characters with safe escape
// sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c :
                '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

// Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

// If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

// If we were called with a replacer function, then call the replacer to
// obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

// What happens next depends on the value's type.

        switch (typeof value) {
        case 'string':
            return quote(value);

        case 'number':

// JSON numbers must be finite. Encode non-finite numbers as null.

            return isFinite(value) ? String(value) : 'null';

        case 'boolean':
        case 'null':

// If the value is a boolean or null, convert it to a string. Note:
// typeof null does not produce 'null'. The case is included here in
// the remote chance that this gets fixed someday.

            return String(value);

// If the type is 'object', we might be dealing with an object or an array or
// null.

        case 'object':

// Due to a specification blunder in ECMAScript, typeof null is 'object',
// so watch out for that case.

            if (!value) {
                return 'null';
            }

// Make an array to hold the partial results of stringifying this object value.

            gap += indent;
            partial = [];

// Is the value an array?

            if (Object.prototype.toString.apply(value) === '[object Array]') {

// The value is an array. Stringify every element. Use null as a placeholder
// for non-JSON values.

                length = value.length;
                for (i = 0; i < length; i += 1) {
                    partial[i] = str(i, value) || 'null';
                }

// Join all of the elements together, separated with commas, and wrap them in
// brackets.

                v = partial.length === 0 ? '[]' : gap ?
                    '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' :
                    '[' + partial.join(',') + ']';
                gap = mind;
                return v;
            }

// If the replacer is an array, use it to select the members to be stringified.

            if (rep && typeof rep === 'object') {
                length = rep.length;
                for (i = 0; i < length; i += 1) {
                    if (typeof rep[i] === 'string') {
                        k = rep[i];
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            } else {

// Otherwise, iterate through all of the keys in the object.

                for (k in value) {
                    if (Object.prototype.hasOwnProperty.call(value, k)) {
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            }

// Join all of the member texts together, separated with commas,
// and wrap them in braces.

            v = partial.length === 0 ? '{}' : gap ?
                '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' :
                '{' + partial.join(',') + '}';
            gap = mind;
            return v;
        }
    }

// If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

// The stringify method takes a value and an optional replacer, and an optional
// space parameter, and returns a JSON text. The replacer can be a function
// that can replace values, or an array of strings that will select the keys.
// A default replacer method can be provided. Use of the space parameter can
// produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

// If the space parameter is a number, make an indent string containing that
// many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

// If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

// If there is a replacer, it must be a function or an array.
// Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

// Make a fake root object containing our value under the key of ''.
// Return the result of stringifying the value.

            return str('', {'': value});
        };
    }


// If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

// The parse method takes a text and an optional reviver function, and returns
// a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

// The walk method is used to recursively walk the resulting structure so
// that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


// Parsing happens in four stages. In the first stage, we replace certain
// Unicode characters with escape sequences. JavaScript handles many characters
// incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

// In the second stage, we run the text against regular expressions that look
// for non-JSON patterns. We are especially concerned with '()' and 'new'
// because they can cause invocation, and '=' because it can cause mutation.
// But just to be safe, we want to reject all unexpected forms.

// We split the second stage into 4 regexp operations in order to work around
// crippling inefficiencies in IE's and Safari's regexp engines. First we
// replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
// replace all simple value tokens with ']' characters. Third, we delete all
// open brackets that follow a colon or comma or that begin the text. Finally,
// we look to see that the remaining characters are only whitespace or ']' or
// ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

// In the third stage we use the eval function to compile the text into a
// JavaScript structure. The '{' operator is subject to a syntactic ambiguity
// in JavaScript: it can begin a block or an object literal. We wrap the text
// in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

// In the optional fourth stage, we recursively walk the new structure, passing
// each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({'': j}, '') : j;
            }

// If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
}());
�?**
*
*  Base64 encode / decode
*  http://www.webtoolkit.info/
*
**/

var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function(input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function(input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function(string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function(utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

};
// FineUI应用程序�?
var F = function (cmpName) {
    return Ext.getCmp(cmpName);
};

F.state = function (cmp, state) {
    F.util.setFState(cmp, state);
};

F.enable = function (id) {
    F.util.enableSubmitControl(id);
};

F.disable = function (id) {
    F.util.disableSubmitControl(id);
};

F.target = function (target) {
    return F.util.getTargetWindow(target);
};

F.alert = function () {
    F.util.alert.apply(window, arguments);
};

F.init = function () {
	F.util.init.apply(window, arguments);
};

F.load = function () {
    F.util.load.apply(window, arguments);
};

F.ready = function () {
    F.util.ready.apply(window, arguments);
};

F.ajaxReady = function () {
    F.util.ajaxReady.apply(window, arguments);
    //if (typeof (onAjaxReady) == 'function') {
    //    onAjaxReady();
    //}
};

F.beforeAjax = function () {
    F.util.beforeAjax.apply(window, arguments);
};

F.stop = function () {
    var event = arguments.callee.caller.arguments[0] || window.event;
    F.util.stopEventPropagation(event);
};

F.confirm = function () {
    F.util.confirm.apply(null, arguments);
};

F.toggle = function (el, className) {
    Ext.get(el).toggleCls(className);
};

F.fieldValue = function (cmp) {
    return F.util.getFormFieldValue(cmp);
};

F.customEvent = function (argument, validate) {
    var pmv = F.pagemanager.validate;
    if (validate && pmv) {
        if (!F.util.validForms(pmv.forms, pmv.target, pmv.messagebox)) {
            return false;
        }
    }
    __doPostBack(F.pagemanager.name, argument);
};

F.getHidden = function () {
    return F.util.getHiddenFieldValue.apply(window, arguments);
};
F.setHidden = function () {
    return F.util.setHiddenFieldValue.apply(window, arguments);
};

F.addCSS = function () {
    F.util.addCSS.apply(window, arguments);
};


// 更新EventValidation的�?
F.eventValidation = function (newValue) {
    F.setHidden("__EVENTVALIDATION", newValue);
};


// 更新ViewState的�?
F.viewState = function (viewStateBeforeAJAX, newValue, startIndex) {
    var viewStateHiddenFiledId = '__VIEWSTATE';

    var oldValue = F.getHidden(viewStateHiddenFiledId);
    var viewStateChanged = false;
    if (oldValue !== viewStateBeforeAJAX) {
        viewStateChanged = true;
    }

    if (typeof (newValue) === 'undefined') {
        // AJAX过程中ViewState值没变化
        if (viewStateChanged) {
            F.setHidden(viewStateHiddenFiledId, viewStateBeforeAJAX);
        }
    } else {
        // AJAX过程中ViewState值有变化
        if (Ext.type(startIndex) === 'number' && startIndex > 0) {
            // 只返回startIndex之后的内�?
            if (viewStateChanged) {
                // 无法处理�?
                return false;
            } else {
                F.setHidden(viewStateHiddenFiledId, oldValue.substr(0, startIndex) + newValue);
            }
        } else {
            // 返回完整的ViewState
            F.setHidden(viewStateHiddenFiledId, newValue);
        }
    }

    // 更新成功�?
    return true;
};

// cookie('theme');
// cookie('theme', 'gray');
// cookie('theme', 'gray', { 'expires': 3 });
// expires: �?
// 新增 或�?修改Cookie
F.cookie = function (key, value, options) {
    if (typeof(value) === 'undefined') {
        var cookies = document.cookie ? document.cookie.split('; ') : [];
        var result = key ? '' : {};
        Ext.Array.each(cookies, function (cookie, index) {
            var parts = cookie.split('=');
            var partName = decodeURIComponent(Ext.String.trim(parts[0]));
            var partValue = decodeURIComponent(Ext.String.trim(parts[1]));

            if (key) {
                if (key === partName) {
                    result = partValue;
                    return false;
                }
            } else {
                result[partName] = partValue;
            }
        });
        return result;
    } else {
        // Set cookie
        options = Ext.apply(options || {}, {
            path: '/'
        });

        var expTime;
        if (typeof (options.expires) === 'number') {
            expTime = new Date();
            expTime.setTime(expTime.getTime() + options.expires * 24 * 60 * 60 * 1000);
        }

        document.cookie = [
            encodeURIComponent(key), '=', encodeURIComponent(value),
            options.expires ? '; expires=' + expTime.toUTCString() : '',
            options.path ? '; path=' + options.path : '',
            options.domain ? '; domain=' + options.domain : '',
            options.secure ? '; secure' : ''
        ].join('');
    }
};

// 删除Cookie
F.removeCookie = function (key, options) {
    options = Ext.apply(options || {}, {
        path: '/',
        'expires': -1
    });

    F.cookie(key, '', options);
};


Ext.onReady(function () {

    F.util.triggerLoad();

    
    F.util.triggerReady();


    F.util.hidePageLoading();

});

(function () {

    // 遍历定义�?renderTo 属性的对象
    // callback: 'return false' to prevent loop continue
    function resolveRenderToObj(callback) {
        Ext.ComponentManager.each(function (key, cmp) {
            if (cmp.isXType && cmp.renderTo) {

                var result = callback.apply(cmp, [cmp]);
                if (result === false) {
                    return false; // break
                }

            }
        });
    }


    // 能否访问 iframe 中的 window.F 对象
    function canIFrameWindowAccessed(iframeWnd) {

        // 访问 iframeWnd.F 时，可能出现错误 Blocked a frame with origin "http://fineui.com/" from accessing a cross-origin frame.
        // Blocked：这个问题出现在 http://fineui.com/ 页面加载一�?http://baidu.com/ �?iframe 页面
        try {
            iframeWnd.F;
        } catch (e) {
            return false;
        }

        if (!iframeWnd.F) {
            return false;
        }

        return true;
    }


    // FineUI常用函数域（Utility�?
    F.util = {

        alertTitle: "Alert Dialog",
        confirmTitle: "Confirm Dialog",
        formAlertMsg: "Please provide valid value for {0}!",
        formAlertTitle: "Form Invalid",
        loading: "Loading...",

        // 下拉列表的模�?
        ddlTPL: '<tpl for="."><div class="x-boundlist-item<tpl if="!enabled"> x-boundlist-item-disabled</tpl>">{prefix}{text}</div></tpl>',

        // 初始�?
        init: function (options) { // msgTarget, labelWidth, labelSeparator, blankImageUrl, enableAjaxLoading, ajaxLoadingType, enableAjax, themeName, formChangeConfirm) {
            
			Ext.apply(F, options, {
				language: 'zh_CN',
				msgTarget: 'side',
				labelWidth: 100, 
				labelSeparator: '�?, 
				//blankImageUrl: '', 
				enableAjaxLoading: true, 
				ajaxLoadingType: 'default', 
				enableAjax: true, 
				theme: 'neptune', 
				formChangeConfirm: false,
				ajaxTimeout: 120
			});
			
			
			// Ext.QuickTips.init(true); 在原生的IE7（非IE8下的IE7模式）会有问�?
            // 表现为iframe中的页面出现滚动条时，页面上的所有按钮都不能点击了�?
            // 测试例子在：aspnet/test.aspx
            //Ext.QuickTips.init(false);
            Ext.tip.QuickTipManager.init();

            F.ajax.hookPostBack();

            F.global_enable_ajax = F.enableAjax;

            F.global_enable_ajax_loading = F.enableAjaxLoading;
            F.global_ajax_loading_type = F.ajaxLoadingType;

            // 添加Ajax Loading提示节点
            F.ajaxLoadingDefault = Ext.get(F.util.appendLoadingNode());
            F.ajaxLoadingMask = Ext.create('Ext.LoadMask', Ext.getBody(), { msg: F.util.loading });


            F.form_upload_file = false;
            F.global_disable_ajax = false;
            //F.x_window_manager = new Ext.WindowManager();
            //F.x_window_manager.zseed = 6000;

            F.util.setHiddenFieldValue('F_CHANGED', 'false');
            document.forms[0].autocomplete = 'off';

            Ext.getBody().addCls('f-body');
			
			Ext.Ajax.timeout = F.ajaxTimeout * 1000;

            // 向document.body添加主题�?
            if (F.theme) {
                Ext.getBody().addCls('f-theme-' + F.theme);
            }

            if (Ext.form.field) {
                var fieldPro = Ext.form.field.Base.prototype;
                fieldPro.msgTarget = F.msgTarget;
                fieldPro.labelWidth = F.labelWidth;
                fieldPro.labelSeparator = F.labelSeparator;
                fieldPro.autoFitErrors = true;
            }
            if (Ext.form.CheckboxGroup) {
                var checkboxgroupPro = Ext.form.CheckboxGroup.prototype;
                checkboxgroupPro.msgTarget = F.msgTarget;
                checkboxgroupPro.labelWidth = F.labelWidth;
                checkboxgroupPro.labelSeparator = F.labelSeparator;
                checkboxgroupPro.autoFitErrors = true;
            }

            F.beforeunloadCheck = true;
            // 启用表单改变确认对话�?
            if (F.formChangeConfirm) {
                // 下面这个方法�?Chrome�?Firefox下无�?
                //Ext.EventManager.on(window, 'beforeunload', function (event) {
                window.onbeforeunload = function () {
                    // 允许关闭页面前提示，并且表单改变
                    if (F.beforeunloadCheck && F.util.formChanged()) {
                        return F.wnd.formChangeConfirmMsg;
                    }
                };
            }

            //if (enableBigFont) {
            //    Ext.getBody().addCls('bigfont');
            //}

			/*
            // IE6&7不支持，IE8以上支持"data:image/gif;base64,R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=="
            if (Ext.isIE6 || Ext.isIE7) {
                Ext.BLANK_IMAGE_URL = F.blankImageUrl;
            }
			*/

            // Submit
            F.ready(function () {
                if (F.submitbutton) {
                    Ext.ComponentManager.each(function (key, cmp) {
                        if (cmp.isXType && cmp.renderTo) {
                            if (cmp.isXType('tooltip')) {
                                return true; // continue
                            }

                            if (cmp.isXType('panel') || cmp.isXType('formviewport')) {
                                F.util.registerPanelEnterKey(cmp);
                            }
                        }
                    });
                }

            });



            // 为了防止【页面中只有一个input[type=text]，则回车会提交表单】的问题，现在页面上创建一个input[type=text]的空元素
            F.util.appendFormNode('<input type="text" class="f-input-text-hidden">');

        },

        _readyList: [],
        _ajaxReadyList: [],
        _beforeAjaxList: [],
        _loadList: [],

        ready: function (callback) {
            F.util._readyList.push(callback);
        },
        triggerReady: function () {
            Ext.Array.each(F.util._readyList, function (item, index) {
                item.call(window);
            });
        },


        ajaxReady: function (callback) {
            F.util._ajaxReadyList.push(callback);
        },
        triggerAjaxReady: function () {
            Ext.Array.each(F.util._ajaxReadyList, function (item, index) {
                item.call(window);
            });
        },

        beforeAjax: function (callback) {
            F.util._beforeAjaxList.push(callback);
        },
        triggerBeforeAjax: function () {
            Ext.Array.each(F.util._beforeAjaxList, function (item, index) {
                item.call(window);
            });
        },


        load: function (callback) {
            F.util._loadList.push(callback);
        },
        triggerLoad: function () {
            Ext.Array.each(F.util._loadList, function (item, index) {
                item.call(window);
            });
        },

        setFState: function (cmp, state) {
            if (!cmp || !cmp['f_state']) {
                return;
            }

            var oldValue, newValue, el;
            // 如果state中包含CssClass，也就是在服务器端修改了CssClass属性，则需要首先删除原来的CssClass属性�?
            if (typeof (state['CssClass']) !== 'undefined') {
                newValue = state['CssClass'];
                oldValue = cmp['f_state']['CssClass'];
                if (!oldValue) {
                    oldValue = cmp.initialConfig.cls;
                }
                el = cmp.el;
                el.removeCls(oldValue);
                el.addCls(newValue);
            }

            //if (typeof (state['FormItemClass']) !== 'undefined') {
            //    newValue = state['FormItemClass'];
            //    oldValue = cmp['f_state']['FormItemClass'];
            //    if (!oldValue) {
            //        oldValue = cmp.initialConfig.itemCls;
            //    }
            //    // Search for max 10 depth.
            //    el = cmp.el.findParent('.x-form-item', 10, true);
            //    el.removeCls(oldValue);
            //    el.addCls(newValue);
            //}

            Ext.apply(cmp['f_state'], state);

        },

        stopEventPropagation: function (event) {
            event = event || window.event;
            if (typeof (event.cancelBubble) === 'boolean') {
                event.cancelBubble = true;
            } else {
                event.stopPropagation();
            }
        },

        // 绑定函数的上下文
        bind: function (fn, scope) {
            return function () {
                return fn.apply(scope, arguments);
            };
        },

        // 在页面上查找id为findId的节点，替换成replaceHtml
        replace: function (findId, replaceHtml) {
            // 在findId外面添加一个DIV层，然后更新此wrapper的InnerHTML
            var findedControl = Ext.get(findId);
            if (findedControl) {
                var wrapper = findedControl.wrap().update(replaceHtml);
                // 将新增的节点移到wrapper上面
                wrapper.first().insertBefore(wrapper);
                // 然后删除wrapper
                wrapper.remove();
            }
        },

        // 隐藏PageLoading节点
        hidePageLoading: function () {
            /*
            if (fadeOut) {
                Ext.get("loading").remove();
                Ext.get("loading-mask").fadeOut({ remove: true });
            }
            else {
                Ext.get("loading").remove();
                Ext.get("loading-mask").remove();
            }
            */

            Ext.get("loading").hide();
            Ext.get("loading-mask").hide();
        },


        // 去掉字符串中的html标签
        stripHtmlTags: function (str) {
            return str.replace(/<[^>]*>/g, "");
        },


        // 弹出Alert对话�?
        alert: function (msg, title, icon, okscript) {
            title = title || F.util.alertTitle;
            icon = icon || Ext.MessageBox.INFO;
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                buttons: Ext.MessageBox.OK,
                icon: icon,
                fn: function (buttonId) {
                    if (buttonId === "ok") {
                        if (typeof (okscript) === "function") {
                            okscript.call(window);
                        }
                    }
                }
            });
        },

        // 向页面添加Loading...节点
        appendLoadingNode: function () {
            return F.util.appendFormNode({ tag: 'div', id: 'f_ajax_loading', cls: 'f-ajax-loading', html: F.util.loading });
        },

        // 向页面的 form 节点最后添加新的节�?
        appendFormNode: function (htmlOrObj) {
            return Ext.DomHelper.append(document.forms[0], htmlOrObj);
        },

        // 向页面添加一个隐藏字段，如果已经存在则更新�?
        setHiddenFieldValue: function (fieldId, fieldValue) {
            var itemNode = Ext.get(fieldId);
            if (!itemNode) {
                // Ext.DomHelper.append 有问题，例如下面这个例子得到的结果是错的；变通一下，先插入节点，在设置节点的值�?
                // Ext.DomHelper.append(document.forms[0], { tag: "input", type: "hidden", value: '{"X_Items":[["Value1","可选项1",1],["Value2","可选项2（不可选择�?,0],["Value3","可选项3（不可选择�?,0],["Value4","可选项4",1],["Value5","可选项5",1],["Value6","可选项6",1],["Value7","可选择�?",1],["Value8","可选择�?",1],["Value9","可选择�?",1]],"SelectedValue":"Value1"}'});
                // 上面的这个字符串，在IETest的IE8模式下会变成�?
                // {"DropDownList1":{"X_Items":[["Value1","\u9009\u9879 1",1],["Value2","\u9009\u9879 2\uff08\u4e0d\u53ef\u9009\u62e9\uff09",0],["Value3","\u9009\u9879 3\uff08\u4e0d\u53ef\u9009\u62e9\uff09",0],["Value4","\u9009\u9879 4",1],["Value5","\u9009\u9879 5",1],["Value6","\u9009\u9879 6",1],["Value7","\u9009\u9879 7",1],["Value8","\u9009\u9879 8",1],["Value9","\u9009\u9879 9",1]],"SelectedValue":"Value1"}}

                F.util.appendFormNode({ tag: "input", type: "hidden", id: fieldId, name: fieldId });
                Ext.get(fieldId).dom.value = fieldValue;
            }
            else {
                itemNode.dom.value = fieldValue;
            }
        },
        // 从表单中删除隐藏字段
        removeHiddenField: function (fieldId) {
            var itemNode = Ext.get(fieldId);
            if (itemNode) {
                itemNode.remove();
            }
        },
        // 获取页面中一个隐藏字段的�?
        getHiddenFieldValue: function (fieldId) {
            var itemNode = Ext.get(fieldId);
            if (itemNode) {
                return itemNode.getValue();
            }
            return null;
        },

        // 禁用提交按钮（在回发之前禁用以防止重复提交）
        disableSubmitControl: function (controlClientID) {
            F(controlClientID).disable();
            F.util.setHiddenFieldValue('F_TARGET', controlClientID);
        },
        // 启用提交按钮（在回发之后启用提交按钮�?
        enableSubmitControl: function (controlClientID) {
            F(controlClientID).enable();
            F.util.setHiddenFieldValue('F_TARGET', '');
        },



        /*
        // 更新ViewState的�?
        updateViewState: function (newValue, startIndex, gzipped) {
            if (typeof (startIndex) === 'boolean') {
                gzipped = startIndex;
                startIndex = -1;
            }

            var viewStateHiddenFiledID = "__VIEWSTATE";
            if (gzipped) {
                viewStateHiddenFiledID = "__VIEWSTATE_GZ";
            }

            var oldValue = F.util.getHiddenFieldValue(viewStateHiddenFiledID);
            if (Ext.type(startIndex) == "number" && startIndex > 0) {
                if (startIndex < oldValue.length) {
                    oldValue = oldValue.substr(0, startIndex);
                }
            } else {
                // Added on 2011-5-2, this is a horrible mistake.
                oldValue = '';
            }
            F.util.setHiddenFieldValue(viewStateHiddenFiledID, oldValue + newValue);
        },

        // 更新EventValidation的�?
        updateEventValidation: function (newValue) {
            F.util.setHiddenFieldValue("__EVENTVALIDATION", newValue);
        },
        */

        // 设置页面状态是否改�?
        setPageStateChanged: function (changed) {
            var pageState = Ext.get("F_CHANGED");
            if (pageState) {
                pageState.dom.value = changed;
            }
        },

        // 页面状态是否改�?
        isPageStateChanged: function () {
            var pageState = Ext.get("F_CHANGED");
            if (pageState && pageState.getValue() == "true") {
                return true;
            }
            return false;
        },


        // 阻止页面关闭（页面中iframe内的表单已改变，或者页面中iframe定义了beforeunload�?
        preventPageClose: function (el) {
            var me = this;

            // 是否阻止关闭
            var preventClose = false;

            var iframeEls;
            if (el) {
                iframeEls = el.select('iframe');
            } else {
                iframeEls = Ext.select('iframe');
            }

            iframeEls.each(function (iframeEl) {
                var iframeWnd = iframeEl.dom.contentWindow;

                if (!canIFrameWindowAccessed(iframeWnd)) {
                    return true; // continue
                }

                if (iframeWnd && iframeWnd.F) {
                    var iframeF = iframeWnd.F;

                    // 启用表单改变确认对话�?并且 表单已改�?
                    if (iframeF.formChangeConfirm && iframeF.util.formChanged()) {
                        // 阻止关闭当前面板
                        if (!window.confirm(F.wnd.formChangeConfirmMsg)) {
                            preventClose = true;
                            return false; // break
                        } else {
                            // 没有阻止，不要在触发 $(window).beforeunload 事件�?
                            iframeF.beforeunloadCheck = false;
                        }
                    }

                    /*
                    // 是否自定义了 beforeunload 事件
                    var beforeunloadCallbacks = iframeF.util._fjs_getEvent('beforeunload');
                    if (beforeunloadCallbacks) {
                        for (var i = 0, count = beforeunloadCallbacks.length; i < count; i++) {
                            var beforeunloadCallback = beforeunloadCallbacks[i];

                            var confirmMsg = beforeunloadCallback.apply(iframeWnd);
                            if (confirmMsg) {
                                // 阻止关闭当前面板
                                if (!window.confirm(confirmMsg)) {
                                    preventClose = true;
                                    return false; // break
                                } else {
                                    // 没有阻止，不要在触发 $(window).beforeunload 事件�?
                                    iframeF.beforeunloadCheck = false;
                                }
                            }
                        }
                    }
                    */

                    // 子页面是否阻止关�?
                    var childrenPreventClose = iframeF.util.preventPageClose();
                    if (childrenPreventClose) {

                        // 被子页面阻止了，则恢复父页面�?beforeunloadCheck 标识
                        iframeF.beforeunloadCheck = true;

                        preventClose = true;
                        return false; // break
                    }
                }

            });

            return preventClose;
        },

        // 页面中表单字段是否改�?
        formChanged: function () {
            var changed = false;
            resolveRenderToObj(function (obj) {
                if (obj.isXType('container') && obj.f_isDirty()) {
                    changed = true;
                    return false; // break
                }
            });

            return changed;
        },


        // 验证多个表单，返回数组[是否验证通过，第一个不通过的表单字段]
        validForms: function (forms, targetName, showBox) {
            var target = F.util.getTargetWindow(targetName);
            var valid = true;
            var firstInvalidField = null;
            for (var i = 0; i < forms.length; i++) {
                var result = F(forms[i]).f_isValid();
                if (!result[0]) {
                    valid = false;
                    if (firstInvalidField == null) {
                        firstInvalidField = result[1];
                    }
                }
            }

            if (!valid) {
                if (showBox) {
                    var alertMsg = Ext.String.format(F.util.formAlertMsg, firstInvalidField.fieldLabel);
                    target.F.util.alert(alertMsg, F.util.formAlertTitle, Ext.MessageBox.INFO);
                }
                return false;
            }
            return true;
        },


        // 判断隐藏字段值（数组）是否包含value
        isHiddenFieldContains: function (domId, testValue) {
            testValue += "";
            var domValue = Ext.get(domId).dom.value;
            if (domValue === "") {
                //console.log(domId);
                return false;
            }
            else {
                var sourceArray = domValue.split(",");
                return Ext.Array.indexOf(sourceArray, testValue) >= 0 ? true : false;
            }
        },


        // 将一个字符添加到字符列表中，�?添加到[5,3,4]
        addValueToHiddenField: function (domId, addValue) {
            addValue += "";
            var domValue = Ext.get(domId).dom.value;
            if (domValue == "") {
                Ext.get(domId).dom.value = addValue + "";
            }
            else {
                var sourceArray = domValue.split(",");
                if (Ext.Array.indexOf(sourceArray, addValue) < 0) {
                    sourceArray.push(addValue);
                    Ext.get(domId).dom.value = sourceArray.join(",");
                }
            }
        },


        // 从字符列表中移除一个字符，�?从dom的�?5,3,4,2"移除
        removeValueFromHiddenField: function (domId, addValue) {
            addValue += "";
            var domValue = Ext.get(domId).dom.value;
            if (domValue != "") {
                var sourceArray = domValue.split(",");
                if (Ext.Array.indexOf(sourceArray, addValue) >= 0) {
                    sourceArray = sourceArray.remove(addValue);
                    Ext.get(domId).dom.value = sourceArray.join(",");
                }
            }
        },


        // 取得隐藏字段的�?
        getHiddenFieldValue: function (fieldId) {
            var itemNode = Ext.get(fieldId);
            if (!itemNode) {
                return "";
            }
            else {
                return itemNode.dom.value;
            }
        },


        // 取得表单字段的�?
        getFormFieldValue: function (cmp) {
            if (typeof (cmp) === 'string') {
                cmp = F(cmp);
            }
            var value = cmp.getValue();
            if (cmp.isXType('displayfield')) {
                value = value.replace(/<\/?span[^>]*>/ig, '');
            }
            return value;
        },


        // 由target获取window对象
        getTargetWindow: function (target) {
            var wnd = window;
            if (target === '_self') {
                wnd = window;
            } else if (target === '_parent') {
                wnd = parent;
            } else if (target === '_top') {
                wnd = top;
            }
            return wnd;
        },


        // 预加载图�?
        preloadImages: function (images) {
            var imageInstance = [];
            for (var i = 0; i < images.length; i++) {
                imageInstance[i] = new Image();
                imageInstance[i].src = images[i];
            }
        },

        hasCSS: function (id) {
            return !!Ext.get(id);
        },

        addCSS: function (id, content, isCSSFile) {

            // 如果此节点已经存在，则先删除此节�?
            var node = Ext.get(id);
            if (node) {
                Ext.removeNode(node.dom);
            }

            var ss1;

            if (isCSSFile) {
                ss1 = document.createElement('link');
                ss1.setAttribute('type', 'text/css');
                ss1.setAttribute('rel', 'stylesheet');
                ss1.setAttribute('id', id);
                ss1.setAttribute('href', content);
            } else {
                // Tricks From: http://www.phpied.com/dynamic-script-and-style-elements-in-ie/
                ss1 = document.createElement("style");
                ss1.setAttribute("type", "text/css");
                ss1.setAttribute("id", id);
                if (ss1.styleSheet) {   // IE
                    ss1.styleSheet.cssText = content;
                } else {                // the world
                    var tt1 = document.createTextNode(content);
                    ss1.appendChild(tt1);
                }
            }

            var hh1 = document.getElementsByTagName("head")[0];
            hh1.appendChild(ss1);
        },

        /*
        // 在启用AJAX的情况下，使所有的Asp.net的提交按钮（type="submit"）不要响应默认的submit行为，而是自定义的AJAX
        makeAspnetSubmitButtonAjax: function (buttonId) {
        
        // 低版本IE浏览器不允许使用JS修改input标签的type属性，导致此函数无�?
        function resetButton(button) {
        button.set({ "type": "button" });
        button.addListener("click", function (event, el) {
        __doPostBack(el.getAttribute("name"), "");
        event.stopEvent();
        });
        }
        
        if (typeof (buttonId) === "undefined") {
        Ext.Array.each(Ext.DomQuery.select("input[type=submit]"), function (item, index) {
        resetButton(Ext.get(item));
        });
        } else {
        var button = Ext.get(buttonId);
        if (button.getAttribute("type") === "submit") {
        resetButton(button);
        }
        }
        
        },
        
        */

        htmlEncode: function (str) {
            var div = document.createElement("div");
            div.appendChild(document.createTextNode(str));
            return div.innerHTML;
        },

        htmlDecode: function (str) {
            var div = document.createElement("div");
            div.innerHTML = str;
            return div.innerHTML;
        },


        // Whether a object is empty (With no property) or not.
        // 可以使用 Ext.Object.isEmpty
        isObjectEmpty: function (obj) {
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    return false;
                }
            }
            return true;
        },

        // Convert an array to object.
        // ['Text', 'Icon']  -> {'Text':true, 'Icon': true}
        arrayToObject: function (arr) {
            var obj = {};
            Ext.Array.each(arr, function (item, index) {
                obj[item] = true;
            });
            return obj;
        },

        hideScrollbar: function () {
            if (Ext.isIE) {
                window.document.body.scroll = 'no';
            } else {
                window.document.body.style.overflow = 'hidden';
            }
        },


        // 动态添加一个标签页
        // mainTabStrip�?选项卡实�?
        // id�?选项卡ID
        // url: 选项卡IFrame地址 
        // text�?选项卡标�?
        // icon�?选项卡图�?
        // addTabCallback�?创建选项卡前的回调函数（接受tabConfig参数�?
        // refreshWhenExist�?添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        addMainTab: function (mainTabStrip, id, url, text, icon, createToolbar, refreshWhenExist) {
            var iconId, iconCss, tabId, currentTab, tabConfig;

            // 兼容 addMainTab(mainTabStrip, treeNode, addTabCallback, refreshWhenExist) 调用方式
            if (typeof (id) !== 'string') {
                refreshWhenExist = text;
                createToolbar = url;
                url = id.data.href;
                icon = id.data.icon;
                text = id.data.text;

                id = id.getId();
            }

            //var href = node.attributes.href;
            if (icon) {
                iconId = icon.replace(/\W/ig, '_');
                if (!F.util.hasCSS(iconId)) {
                    iconCss = [];
                    iconCss.push('.');
                    iconCss.push(iconId);
                    iconCss.push('{background-image:url("');
                    iconCss.push(icon);
                    iconCss.push('")}');
                    F.util.addCSS(iconId, iconCss.join(''));
                }
            }
            // 动态添加一个带工具栏的标签�?
            //tabId = 'dynamic_added_tab' + id.replace('__', '-');
            currentTab = mainTabStrip.getTab(id);
            if (!currentTab) {
                tabConfig = {
                    'id': id,
                    'url': url,
                    'title': text,
                    'closable': true,
                    'bodyStyle': 'padding:0px;'
                };
                if (icon) {
                    tabConfig['iconCls'] = iconId;
                }

                if (createToolbar) {
                    var addTabCallbackResult = createToolbar.apply(window, [tabConfig]);
                    // 兼容之前的方法，函数返回值如果不为空，则将返回值作为顶部工具条实例
                    if (addTabCallbackResult) {
                        tabConfig['tbar'] = addTabCallbackResult;
                    }
                }
                mainTabStrip.addTab(tabConfig);
            } else {
                mainTabStrip.setActiveTab(currentTab);
                currentTab.setTitle(text);
                if (icon) {
                    currentTab.setIconCls(iconId);
                }
                if (refreshWhenExist) {
                    var iframeNode = currentTab.body.query('iframe')[0];
                    if (iframeNode) {
                        if (url) {
                            iframeNode.contentWindow.location.href = url;
                        } else {
                            iframeNode.contentWindow.location.reload();
                        }
                    }
                }

            }
        },

        // 初始化左侧树（或者手风琴+树）与右侧选项卡控件的交互
        // treeMenu�?主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
        // mainTabStrip�?选项卡实�?
        // createToolbar�?创建选项卡前的回调函数（接受tabConfig参数�?
        // updateLocationHash: 切换Tab时，是否更新地址栏Hash�?
        // refreshWhenExist�?添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
        // hashWindow：需要更新Hash值的窗口对象，默认为当前window
        initTreeTabStrip: function (treeMenu, mainTabStrip, createToolbar, updateLocationHash, refreshWhenExist, refreshWhenTabChange, hashWindow) {
            if (!hashWindow) {
                hashWindow = window;
            }

            // 注册树的节点点击事件
            function registerTreeClickEvent(treeInstance) {
                treeInstance.on('itemclick', function (view, record, item, index, event) {
                    var href = record.data.href;

                    // record.isLeaf()
                    // 不管当前节点是否子节点，只要�?href 属性，都需要打开一个新Tab
                    if (href) {
                        // 阻止事件传播
                        event.stopEvent();

                        if (updateLocationHash) {
                            // 修改地址�?
                            hashWindow.location.hash = '#' + href;
                        }

                        // 新增Tab节点
                        F.util.addMainTab(mainTabStrip, record, createToolbar, refreshWhenExist);
                    }
                });
            }

            // treeMenu可能是Accordion或者Tree
            if (treeMenu.getXType() === 'panel') {
                treeMenu.items.each(function (item) {
                    var tree = item.items.getAt(0);
                    if (tree && tree.getXType() === 'treepanel') {
                        registerTreeClickEvent(tree);
                    }
                });
            } else if (treeMenu.getXType() === 'treepanel') {
                registerTreeClickEvent(treeMenu);
            }

            // 切换主窗口的Tab
            mainTabStrip.on('tabchange', function (tabStrip, tab) {
                var tabHash = '#' + (tab.url || '');

                // 只有当浏览器地址栏的Hash值和将要改变的不一样时，才进行如下两步处理�?
                // 1. 更新地址栏Hash�?
                // 2. 刷新Tab内的IFrame
                if (tabHash !== hashWindow.location.hash) {

                    if (updateLocationHash) {
                        hashWindow.location.hash = tabHash;
                    }

                    if (refreshWhenTabChange) {
                        var iframeNode = tab.body.query('iframe')[0];
                        if (iframeNode) {
                            var currentLocationHref = iframeNode.contentWindow.location.href;
                            if (/^http(s?):\/\//.test(currentLocationHref)) {
                                iframeNode.contentWindow.location.reload();
                            }
                        }
                    }
                }

            });


            // 页面第一次加载时，根据URL地址在主窗口加载页面
            var HASH = hashWindow.location.hash.substr(1);
            if (HASH) {
                var FOUND = false;

                function initTreeMenu(treeInstance, node) {
                    var i, currentNode, nodes, node, path;
                    if (!FOUND && node.hasChildNodes()) {
                        nodes = node.childNodes;
                        for (i = 0; i < nodes.length; i++) {
                            currentNode = nodes[i];
                            if (currentNode.isLeaf()) {
                                if (currentNode.data.href === HASH) {
                                    path = currentNode.getPath();
                                    treeInstance.expandPath(path); //node.expand();
                                    treeInstance.selectPath(path); // currentNode.select();
                                    F.util.addMainTab(mainTabStrip, currentNode, createToolbar);
                                    FOUND = true;
                                    return;
                                }
                            } else {
                                arguments.callee(treeInstance, currentNode);
                            }
                        }
                    }
                }

                if (treeMenu.getXType() === 'panel') {
                    treeMenu.items.each(function (item) {
                        var tree = item.items.getAt(0);
                        if (tree && tree.getXType() === 'treepanel') {
                            initTreeMenu(tree, tree.getRootNode());

                            // 找到树节�?
                            if (FOUND) {
                                item.expand();
                                return false;
                            }
                        }
                    });
                } else if (treeMenu.getXType() === 'treepanel') {
                    initTreeMenu(treeMenu, treeMenu.getRootNode());
                }
            }

        },

        // 复选框分组处理
        resolveCheckBoxGroup: function (name, xstateContainer, isradiogroup) {
            var items = [], i, count, xitem, xitemvalue, xitems, xselectedarray, xselected, xchecked, xitemname;

            xitems = xstateContainer.F_Items;
            xselectedarray = xstateContainer.SelectedValueArray;
            xselected = xstateContainer.SelectedValue;

            if (xitems && xitems.length > 0) {
                for (i = 0, count = xitems.length; i < count; i++) {
                    xitem = xitems[i];
                    xitemvalue = xitem[1];
                    xchecked = false;
                    if (!isradiogroup) {
                        // xselectedarray 可能是undefined, [], ["value1", "value2"]
                        if (xselectedarray) {
                            xchecked = (Ext.Array.indexOf(xselectedarray, xitemvalue) >= 0) ? true : false;
                        }
                        xitemname = name + '_' + i;
                    } else {
                        xchecked = (xselected === xitemvalue) ? true : false;
                        xitemname = name;
                    }
                    items.push({
                        'inputValue': xitemvalue,
                        'boxLabel': xitem[0],
                        'name': xitemname,
                        'checked': xchecked
                    });
                }
            }
            /*
            else {
                items.push({
                    'inputValue': "tobedeleted",
                    'boxLabel': "&nbsp;",
                    'name': "tobedeleted"
                });
            }
            */
            return items;

        },

        // 防止在短时间内，同一GroupName的单选框触发两次事件
        // 用于 MenuCheckBox �?RadioButton
        checkGroupLastTime: function (groupName) {
            var checkName = groupName + '_lastupdatetime';
            var checkValue = F.util[checkName];
            F.util[checkName] = new Date();
            if (typeof (checkValue) === 'undefined') {
                return true;
            } else {
                if ((new Date() - checkValue) < 100) {
                    return false;
                } else {
                    return true;
                }
            }
        },

        // 对话框图�?
        getMessageBoxIcon: function (iconShortName) {
            var icon = Ext.MessageBox.WARNING;
            if (iconShortName === 'info') {
                icon = Ext.MessageBox.INFO;
            } else if (iconShortName === 'warning') {
                icon = Ext.MessageBox.WARNING;
            } else if (iconShortName === 'question') {
                icon = Ext.MessageBox.QUESTION;
            } else if (iconShortName === 'error') {
                icon = Ext.MessageBox.ERROR;
            }
            return icon;
        },

        // 确认对话�?
        confirm: function (targetName, title, msg, okScript, cancelScript, iconShortName) {
            var wnd = F.util.getTargetWindow(targetName);
            var icon = F.util.getMessageBoxIcon(iconShortName);
            wnd.Ext.MessageBox.show({
                title: title || F.util.confirmTitle,
                msg: msg,
                buttons: Ext.MessageBox.OKCANCEL,
                icon: icon,
                fn: function (btn) {
                    if (btn == 'cancel') {
                        if (cancelScript) {
                            if (typeof (cancelScript) === 'string') {
                                new Function(cancelScript)();
                            } else {
                                cancelScript.apply(wnd);
                            }
                        } else {
                            return false;
                        }
                    } else {
                        if (okScript) {
                            if (typeof (okScript) === 'string') {
                                new Function(okScript)();
                            } else {
                                okScript.apply(wnd);
                            }
                        } else {
                            return false;
                        }
                    }
                }
            });
        },



        summaryType: function (gridId) {
            return function (records, dataIndex) {
                var summary = F(gridId).f_state['SummaryData'];
                if (summary) {
                    var value = summary[dataIndex];
                    if (typeof (value) !== 'undefined') {
                        return value;
                    }
                }
                return '';
            };
        },

        // 表单字段内按回车键触发提交按�?
        registerPanelEnterKey: function (panel) {
            if (F.submitbutton) {
                Ext.create('Ext.util.KeyNav', panel.el, {
                    enter: function (e) {
                        var el = Ext.Element.getActiveElement();
                        if (el.type !== 'textarea') {
                            F(F.submitbutton).el.dom.click();
                        }
                    },
                    scope: panel
                });
            }
        },

        reset: function () {
            Ext.ComponentManager.each(function (key, cmp) {
                if (cmp.isXType && cmp.isXType('panel') && cmp.renderTo) {
                    cmp.f_reset();
                }
            });
        },


        isDate: function (value) {
            return Object.prototype.toString.call(value) === '[object Date]';
        },

        resolveGridDateToString: function (fields, fieldName, fieldValue) {
            var i, fieldConfig, result = fieldValue;
            for (i = 0, count = fields.length; i < count; i++) {
                fieldConfig = fields[i];
                if (fieldConfig.name === fieldName && fieldConfig.type === 'date' && fieldConfig.dateFormat) {
                    result = Ext.util.Format.date(fieldValue, fieldConfig.dateFormat);
                }
            }
            return result;
        },


        noop: function () { }

    };




})();�?
(function () {

    F.ajax = {

        timeoutErrorMsg: "Request timeout, please refresh the page and try again!",
        errorMsg: "Error! {0} ({1})",
        errorWindow: null,

        hookPostBack: function () {
            if (typeof (__doPostBack) != 'undefined') {
                __doPostBack = f__doPostBack;
            }
        }

    };

    function enableAjax() {
        if (typeof (F.control_enable_ajax) === 'undefined') {
            return F.global_enable_ajax;
        }
        return F.control_enable_ajax;
    }

    function enableAjaxLoading() {
        if (typeof (F.control_enable_ajax_loading) === 'undefined') {
            return F.global_enable_ajax_loading;
        }
        return F.control_enable_ajax_loading;
    }

    function ajaxLoadingType() {
        if (typeof (F.control_ajax_loading_type) === 'undefined') {
            return F.global_ajax_loading_type;
        }
        return F.control_ajax_loading_type;
    }


    function f__doPostBack_internal() {
        //if (typeof (F.util.beforeAjaxPostBackScript) === 'function') {
        //    F.util.beforeAjaxPostBackScript();
        //}
        F.util.triggerBeforeAjax();


        function ajaxSuccess(data, viewStateBeforeAJAX) {
            /*
			try {
				new Function(data)();
			} catch (e) {
				createErrorWindow({
					statusText: "Execute JavaScript Exception",
					status: -1,
					responseText: util.htmlEncode(data)
				});
			}
			*/
            new Function('__VIEWSTATE', data)(viewStateBeforeAJAX);

            // 有可能响应返回后即关闭本窗体
            if (F && F.util) {
                F.util.triggerAjaxReady();
            }
        }

        // Ext.encode will convert Chinese characters. Ext.encode({a:"你好"}) => '{"a":"\u4f60\u597d"}'
        // We will include the official JSON object from http://json.org/
        // 现在还是用的 Ext.encode，在 IETester�?IE8�?JSON.stringify 生成的中文是\u9009\u9879形式�?
        //F.util.setHiddenFieldValue('F_STATE', encodeURIComponent(JSON.stringify(getFState())));

        var fstate = Ext.encode(getFState());
        if (Ext.isIE6 || Ext.isIE7) {
            F.util.setHiddenFieldValue('F_STATE_URI', 'true');
            fstate = encodeURIComponent(fstate);
        } else {
            fstate = Base64.encode(fstate);
        }
        F.util.setHiddenFieldValue('F_STATE', fstate);
        //F.util.setHiddenFieldValue('F_STATE', encodeURIComponent(Ext.encode(getFState())));
        if (!enableAjax()) {
            // 当前请求结束后必须重�?F.control_enable_ajax
            F.control_enable_ajax = undefined;
            F.util.setHiddenFieldValue('F_AJAX', 'false');
            theForm.submit();
        } else {
            // 当前请求结束后必须重�?F.control_enable_ajax
            F.control_enable_ajax = undefined;
            F.util.setHiddenFieldValue('F_AJAX', 'true');
            var url = document.location.href;
            var urlHashIndex = url.indexOf('#');
            if (urlHashIndex >= 0) {
                url = url.substring(0, urlHashIndex);
            }

            var viewStateBeforeAJAX = F.util.getHiddenFieldValue('__VIEWSTATE');

            Ext.Ajax.request({
                form: theForm.id,
                url: url,
                isUpload: F.form_upload_file,
                //params: serializeForm(theForm) + '&X_AJAX=true',
                success: function (data) {
                    var scripts = data.responseText;

                    
                    if (scripts && F.form_upload_file) {
                        // 文件上传时，输出内容经过encodeURIComponent编码（在ResponseFilter中的Close函数中）
                        //scripts = scripts.replace(/<\/?pre[^>]*>/ig, '');
                        scripts = decodeURIComponent(scripts);
                    }


                    // 因为这里调用后（可能会关闭当前页面），extjs还有代码要执行（Ext.callback...），所以这里要延迟一下，�?extjs 代码执行完毕后再执行这里代码
                    window.setTimeout(function () {
                        ajaxSuccess(scripts, viewStateBeforeAJAX);
                        /*
                        if (scripts) {
                            if (F.form_upload_file) {
                                // 文件上传时，输出内容经过encodeURIComponent编码（在ResponseFilter中的Close函数中）
                                //scripts = scripts.replace(/<\/?pre[^>]*>/ig, '');
                                scripts = decodeURIComponent(scripts);
                            }


                            new Function(scripts)();
                            

                        }
                        // 有可能响应返回后即关闭本窗体
                        if (F && F.util) {
                            F.util.triggerAjaxReady();
                        }
                        */
                    }, 100);
                },
                failure: function (data) {
                    var lastDisabledButtonId = F.util.getHiddenFieldValue('F_TARGET');
                    if (lastDisabledButtonId) {
                        F.enable(lastDisabledButtonId);
                    }
                    createErrorWindow(data);
                },
                callback: function (options, success, response) {
                    // AJAX结束时需要清空此字段，否则下一次的type=submit提交（ASP.NET回发方式之一）会被误认为是AJAX提交
                    if (F && F.util) {
                        F.util.setHiddenFieldValue('F_AJAX', 'false');
                    }
                }
            });
        }
    }


    // 如果启用 Ajax，则所有对 __doPostBack 的调用都会到这里�?
    function f__doPostBack(eventTarget, eventArgument) {
        // 回发页面之前延时 100 毫秒，确保页面上的操作完成（比如选中复选框的动作）
        window.setTimeout(function () {
            // theForm variable will always exist, because we invoke the GetPostBackEventReference in PageManager.
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;

                f__doPostBack_internal();
            }
        }, 100);
    }


    function writeContentToIFrame(iframe, content) {
        // http://stackoverflow.com/questions/1477547/getelementbyid-contentdocument-error-in-ie
        // contentWindow is always there.
        if (iframe) {
            var doc = iframe.contentWindow.document;
            if (doc) {
                doc.open();
                doc.write(content);
                doc.close();
            }
        }
    }

    // 创建出错窗口
    function createErrorWindow(data) {
        // 如果是请求超时错误，则弹出简单提醒对话框
        if (data.isTimeout) {
            F.util.alert(F.ajax.timeoutErrorMsg);
            return;
        }

        // 如果响应正文为空，则弹出简单提醒对话框
        if (!data.responseText) {
            F.util.alert(Ext.String.format(F.ajax.errorMsg, data.statusText, data.status));
            return;
        }

        if (!F.ajax.errorWindow) {
            F.ajax.errorWindow = Ext.create('Ext.window.Window', {
                id: "FINEUI_ERROR",
                renderTo: window.body,
                width: 550,
                height: 350,
                border: true,
                animCollapse: true,
                collapsible: false,
                collapsed: false,
                closeAction: "hide",
                plain: false,
                modal: true,
                draggable: true,
                minimizable: false,
                minHeight: 100,
                minWidth: 200,
                resizable: true,
                maximizable: true,
                closable: true
            });
        }

        F.ajax.errorWindow.show();
        F.ajax.errorWindow.body.dom.innerHTML = F.wnd.createIFrameHtml('about:blank', 'FINEUI_ERROR');
        F.ajax.errorWindow.setTitle(Ext.String.format(F.ajax.errorMsg, data.statusText, data.status));
        writeContentToIFrame(F.ajax.errorWindow.body.query('iframe')[0], data.responseText);
    }

    // 序列化表单为 URL 编码字符串，除去 <input type="submit" /> 的按�?
    var extjsSerializeForm = Ext.Element.serializeForm;
    Ext.Element.serializeForm = function (form) {
        var el, originalStr = extjsSerializeForm(form);
        for (var i = 0; i < form.elements.length; i++) {
            el = form.elements[i];
            if (el.type === 'submit') {
                var submitStr = encodeURIComponent(el.name) + '=' + encodeURIComponent(el.value);
                if (originalStr.indexOf(submitStr) == 0) {
                    originalStr = originalStr.replace(submitStr, '');
                } else {
                    originalStr = originalStr.replace('&' + submitStr, '');
                }
            }
        }
        return originalStr;
    };


    function getFState() {
        var state = {};
        Ext.ComponentManager.each(function (key, cmp) {
            if (cmp.isXType) {
                // f_props store the properties which has been changed on server-side or client-side.
                // Every FineUI control should has this property.
                var fstate = cmp['f_state'];
                if (fstate && Ext.isObject(fstate)) {
                    var cmpState = getFStateViaCmp(cmp, fstate);
                    if (!F.util.isObjectEmpty(cmpState)) {
                        state[cmp.id] = cmpState;
                    }
                }
            }
        });
        return state;
    }

    F.ajax.getFState = getFState;

    function getFStateViaCmp(cmp, fstate) {
        var state = {};

        Ext.apply(state, fstate);

        function saveInHiddenField(property, currentValue) {
            // Save this client-changed property in a form hidden field. 
            F.util.setHiddenFieldValue(cmp.id + '_' + property, currentValue);
        }
        function removeHiddenField(property) {
            F.util.removeHiddenField(cmp.id + '_' + property);
        }

        // 如果存在Gzip压缩的属性，就删除原来的属�?
        function resolveGZProperty(property) {
            var gzProperty = property + '_GZ';
            if (state[gzProperty]) {
                delete state[property];
            } else {
                delete state[gzProperty];
            }
        }



        // 有些属性可以在客户端改变，因此需要在每个请求之前计算
        if (cmp.isXType('menucheckitem')) {
            saveInHiddenField('Checked', cmp.checked);
        }

        if (cmp.isXType('checkbox')) {
            // 包含RadioButton
            saveInHiddenField('Checked', cmp.getValue());
        }

        if (cmp.isXType('checkboxgroup')) {
            var selected = cmp.f_getSelectedValues();
            if (selected.length > 0) {
                saveInHiddenField('SelectedValueArray', selected.join(','));
            } else {
                removeHiddenField('SelectedValueArray');
            }
        }

        if (cmp.isXType('panel') || cmp.isXType('fieldset')) {
            saveInHiddenField('Collapsed', cmp.f_isCollapsed());
        }

        if (cmp.isXType('datepicker')) {
            saveInHiddenField('SelectedDate', Ext.Date.format(cmp.getValue(), cmp.initialConfig.format));
        }

        if (cmp.isXType('button')) {
            if (cmp.initialConfig.enableToggle) {
                saveInHiddenField('Pressed', cmp.pressed);
            }
        }

        if (cmp.isXType('grid')) {

            //if (cmp.getPlugin(cmp.id + '_celledit')) {
            if(cmp.f_cellEditing) {
                // 可编辑单元格的表�?
                // 选中单元�?
                saveInHiddenField('SelectedCell', cmp.f_getSelectedCell().join(','));

                //// 新增�?
                //var newAddedRows = cmp.f_getNewAddedRows();
                //if (newAddedRows.length > 0) {
                //    saveInHiddenField('NewAddedRows', newAddedRows.join(','));
                //} else {
                //    removeHiddenField('NewAddedRows');
                //}

                // 修改的数�?
                var modifiedData = cmp.f_getModifiedData();
                if (modifiedData.length > 0) {
                    saveInHiddenField('ModifiedData', Ext.encode(modifiedData));
                } else {
                    removeHiddenField('ModifiedData');
                }

                // 删除的行索引列表
                var deletedRows = cmp.f_getDeletedRows();
                if (deletedRows.length > 0) {
                    saveInHiddenField('DeletedRows', deletedRows.join(','));
                } else {
                    removeHiddenField('DeletedRows');
                }

            } else {
                // 普通的表格
                // 选中行索引列�?
                saveInHiddenField('SelectedRowIndexArray', cmp.f_getSelectedRows().join(','));
            }


            // 隐藏的列索引列表
            var gridHiddenColumns = cmp.f_getHiddenColumns();
            if (gridHiddenColumns.length > 0) {
                saveInHiddenField('HiddenColumns', gridHiddenColumns.join(','));
            } else {
                removeHiddenField('HiddenColumns');
            }

            // 目前States仅用于CheckBoxField
            var gridStates = cmp.f_getStates();
            if (gridStates.length > 0) {
                saveInHiddenField('States', Ext.encode(gridStates));
            } else {
                removeHiddenField('States');
            }

            // 如果存在 GZIPPED 的属性，就用 GZIPPED 属�?
            resolveGZProperty('F_Rows');
        }

        if (cmp.isXType('combo') || cmp.isXType('checkboxgroup') || cmp.isXType('radiogroup')) {

            // 如果存在 GZIPPED 的属性，就用 GZIPPED 属�?
            resolveGZProperty('F_Items');
        }

        if (cmp.isXType('field')) {

            // 如果存在 GZIPPED 的属性，就用 GZIPPED 属�?
            resolveGZProperty('Text');
        }

        if (cmp.isXType('treepanel')) {
            saveInHiddenField('ExpandedNodes', cmp.f_getExpandedNodes(cmp.getRootNode().childNodes).join(','));
            saveInHiddenField('CheckedNodes', cmp.f_getCheckedNodes().join(','));
            saveInHiddenField('SelectedNodeIDArray', cmp.f_getSelectedNodes().join(','));

            // 如果存在 GZIPPED 的属性，就用 GZIPPED 属�?
            resolveGZProperty('F_Nodes');
        }

        if (cmp.isXType('tabpanel')) {
            saveInHiddenField('ActiveTabIndex', cmp.f_getActiveTabIndex());
        }

        if (cmp.isXType('panel') && cmp.getLayout().type === 'accordion') {
            saveInHiddenField('ActivePaneIndex', cmp.f_getActiveIndex());
        }

        if (cmp['f_type'] && cmp['f_type'] === 'tab') {
            saveInHiddenField('Hidden', cmp.tab.isHidden());
        }

        return state;
    }



    // 显示“正在载�?..”的提示信息
    function _showAjaxLoading(ajaxLoadingType) {
        if (_requestCount > 0) {

            if (ajaxLoadingType === "default") {
                F.ajaxLoadingDefault.setStyle('left', (Ext.getBody().getWidth() - F.ajaxLoadingDefault.getWidth()) / 2 + 'px');
                F.ajaxLoadingDefault.show();
            } else {
                F.ajaxLoadingMask.show();
            }

        }
    }

    // 隐藏“正在载�?..”的提示信息
    function _hideAjaxLoading(ajaxLoadingType) {
        if (_requestCount <= 0) {
            _requestCount = 0;

            if (ajaxLoadingType === "default") {
                F.ajaxLoadingDefault.hide();
            } else {
                F.ajaxLoadingMask.hide();
            }

        }
    }

    // 当前 Ajax 的并发请求数
    var _requestCount = 0;

    // 发起 Ajax 请求之前事件处理
    Ext.Ajax.on('beforerequest', function (conn, options) {
        _requestCount++;

        if (!enableAjaxLoading()) {
            // Do nothing
        } else {
            Ext.defer(_showAjaxLoading, 50, window, [ajaxLoadingType()]);
        }
    });

    // Ajax 请求结束
    Ext.Ajax.on('requestcomplete', function (conn, options) {
        _requestCount--;

        if (!enableAjaxLoading()) {
            // ...
        } else {
            Ext.defer(_hideAjaxLoading, 0, window, [ajaxLoadingType()]);
        }
        F.control_enable_ajax_loading = undefined;
        F.control_ajax_loading_type = undefined;
    });

    // Ajax 请求发生异常
    Ext.Ajax.on('requestexception', function (conn, options) {
        _requestCount--;

        if (!enableAjaxLoading()) {
            // ...
        } else {
            Ext.defer(_hideAjaxLoading, 0, window, [ajaxLoadingType()]);
        }
        F.control_enable_ajax_loading = undefined;
        F.control_ajax_loading_type = undefined;
    });






    //        // 不适用于所有Extjs控件（比如Toolbar中放置按钮，这个按钮就没有ownerCt对象�?
    //        // 更新一个Javascript对象
    //        updateObject: function(obj, newObjFunction, renderImmediately) {
    //            var id = obj.id;
    //            if (Ext.type(renderImmediately) == 'boolean' && !renderImmediately) {

    //                // 1.取得父容�?
    //                var owner = obj.ownerCt;
    //                // 2.本控件在父容器的位置
    //                var insertIndex = owner.items.indexOf(obj);
    //                // 3.从父容器中销毁此控件
    //                owner.remove(obj);
    //                // 4.创建新的控件
    //                newObjFunction();
    //                // 5.将新的控件添加到删除的位�?
    //                owner.insert(insertIndex, Ext.getCmp(id));
    //                // 6.父容器重新布局
    //                owner.doLayout();

    //            }
    //            else {

    //                // 1.销毁此控件
    //                obj.destroy();
    //                // 2.新建此控�?
    //                newObjFunction();
    //            }
    //        }

})();�?

(function () {

    // 计算黄金分割点的位置
    // bodySize : 整个页面的Body的大�?
    // windowSize : 窗口的大�?
    function _calculateGoldenPosition(bodySize, windowSize) {
        var top = (bodySize.height - (bodySize.height / 1.618)) - windowSize.height / 2;
        if (top < 0) {
            top = 0;
        }
        var left = (bodySize.width - windowSize.width) / 2;
        if (left < 0) {
            left = 0;
        }
        return { left: left, top: top };
    }

    // 计算中间的位�?
    // bodySize : 整个页面的Body的大�?
    // windowSize : 窗口的大�?
    function _calculateCenterPosition(bodySize, windowSize) {
        var top = (bodySize.height - windowSize.height) / 2;
        if (top < 0) {
            top = 0;
        }
        var left = (bodySize.width - windowSize.width) / 2;
        if (left < 0) {
            left = 0;
        }
        return { left: left, top: top };
    }



    // 创建IFrame节点片段
    function _createIFrameHtml(iframeUrl, iframeName) {
        return '<iframe frameborder="0" style="overflow:auto;height:100%;width:100%;" name="' + iframeName + '" src="' + iframeUrl + '"></iframe>';
    }

    // 获取窗体的外部容�?
    function _getWrapperNode(panel) {
        return Ext.get(panel.el.findParentNode('.x-window-wrapper'));
    }

    // FineUI窗口域（Window�?
    F.wnd = {

        closeButtonTooltip: "Close this window",
        formChangeConfirmMsg: "Current form has been modified, abandon changes?",

        createIFrameHtml: function (iframeUrl, iframeName) {
            return _createIFrameHtml(iframeUrl, iframeName);
        },

        // 窗体定义：Original Panel / Ghost Panel

        // 显示一个弹出窗�?
        // �?panel 实例中，定义了几个自定义属性，用于标示此实例的状态（在PanelBase中定义）
        // 属�?- f_iframe/f_iframe_url/f_iframe_name/f_iframe_loaded
        // panel : 当前弹出的窗体（Ext-Window�?
        // iframeUrl : 弹出窗体中包含的IFrame的地址
        // windowTitle : 弹出窗体的标�?
        // left/top : 弹出窗体的左上角坐标（如果为空字符串，则使用中间位置或黄金分隔位置）
        // isGoldenSection : 弹出窗体位于页面的黄金分隔位�?
        // hiddenHiddenFieldID : 隐藏表单字段记录此窗体是否弹出，也页面回发时保持状态用
        show: function (panel, iframeUrl, windowTitle, left, top, isGoldenSection, hiddenHiddenFieldID, width, height) {
            var target = F.util.getTargetWindow(panel['f_property_target']);
            var guid = panel['f_property_guid'];
            if (window.frameElement && target !== window) {
                // 当前页面在IFrame中（也即�?window.frameElement 存在�?
                // 此弹出窗体需要在父窗口中弹出
                if (!target.F[guid]) {
                    // 父窗口中已经创建了这个Ext-Window对象
                    var wrapper = guid + '_wrapper';
                    if (!target.Ext.get(wrapper)) {
                        target.F.util.appendFormNode('<div class="x-window-wrapper" id="' + wrapper + '"></div>');
                    } else {
                        target.Ext.get(wrapper).dom.innerHTML = '';
                    }
                    // Ext.apply 的第三个参数是default obejct
                    var config = Ext.apply({}, {
                        'renderTo': wrapper,
                        'id': guid,
                        'f_property_window': window,
                        'f_property_ext_window': panel
                    }, panel.initialConfig);

                    // 在父页面中创建一个Ext-Window的幻影（拷贝�?
                    target.F[guid] = target.Ext.create('Ext.window.Window', config);
                }
                panel = target.F[guid];
            }
			
            if (iframeUrl !== '') {
                F.wnd.updateIFrameNode(panel, iframeUrl);
            }
            if (windowTitle != '') {
                panel.setTitle(windowTitle);
            }

			
            if (typeof(width) === 'number' && width) {
                panel.setWidth(width);
            }
			
			if (typeof(height) === 'number' && height) {
                panel.setHeight(height);
            }

			
            Ext.get(hiddenHiddenFieldID).dom.value = 'false';
            panel.show();

            if (left !== '' && top !== '') {
                panel.setPosition(parseInt(left, 10), parseInt(top, 10));
            } else {
                var bodySize = target.window.Ext.getBody().getViewSize();
                var panelSize = panel.getSize(), leftTop;
                if (isGoldenSection) {
                    leftTop = _calculateGoldenPosition(bodySize, panelSize);
                } else {
                    leftTop = _calculateCenterPosition(bodySize, panelSize);
                    //panel.alignTo(target.Ext.getBody(), "c-c");
                }
                panel.setPosition(leftTop.left, leftTop.top);
            }

            if (panel.maximizable) {
                F.wnd.fixMaximize(panel);
            }
        },

        // 获取Ghost Panel实例
        getGhostPanel: function (panel, targetName, guid) {
            if (typeof (targetName) === 'undefined') {
                targetName = panel.f_property_target;   
            }
            if (typeof (guid) === 'undefined') {
                guid = panel.f_property_guid;   
            }
            var target = F.util.getTargetWindow(targetName);
            if (window.frameElement && target !== window) {
                // 从父页面中查找幻影Ext-Window对象
                panel = target.F[guid];
            }
            return panel;
        },

        // 隐藏Ext-Window（比如用户点击了关闭按钮�?
        hide: function (panel, enableIFrame, hiddenHiddenFieldID) {
            var panel = F.wnd.getGhostPanel(panel);

            // 如果返回 false，则说明隐藏操作被阻止了
            if (panel.hide() !== false) {

                // 修改当前页面中记录弹出窗口弹出状态的隐藏表单字段
                Ext.get(hiddenHiddenFieldID).dom.value = 'true';
                // 如果启用IFrame，则清空IFrame的内容，防止下次打开时显示残�?
                if (enableIFrame) {
                    // 如果不加延迟，IE下AJAX会出错，因为在success中已经把当前窗体关闭后，而后面还要继续使用本页面上相关对�?
                    window.setTimeout(function () {
                        panel['f_iframe_loaded'] = false;
                        panel.update("");
                    }, 100);
                }

            }
        },

        // 最大化
        maximize: function (panel) {
            var panel = F.wnd.getGhostPanel(panel);
            panel.maximize();
        },

        // 最小化
        minimize: function (panel) {
            var panel = F.wnd.getGhostPanel(panel);
            panel.minimize();
        },

        // 恢复窗体大小
        restore: function (panel) {
            var panel = F.wnd.getGhostPanel(panel);
            panel.restore();
        },

        // 这是 Extjs 的一�?bug，如�?Window 控件不是渲染�?document.body 中，�?maximize 函数并不能真正的最大化
        // 现在�?Window 控件时渲染在 from 表单里面的一�?DIV 中的
        fixMaximize: function (panel) {
            if (panel.maximized) {
                var target = F.util.getTargetWindow(panel['f_property_target']);
                var bodySize = target.window.Ext.getBody().getViewSize();
                panel.setSize(bodySize.width, bodySize.height);
                // 不要忘记左上角坐�?
                panel.setPosition(0, 0);
            }
        },

        // 创建或更新IFrame节点，同时更新panel实例中的自定义属性�?
        updateIFrameNode: function (panel, iframeUrl) {
            var iframeUrlChanged = false;
            panel = F.wnd.getGhostPanel(panel);
            // 如果此Panel中包含有IFrame
            if (panel && panel['f_iframe']) {
                if (iframeUrl && panel['f_iframe_url'] !== iframeUrl) {
                    panel['f_iframe_url'] = iframeUrl;
                    iframeUrlChanged = true;
                }
                // 如果此Panel中包含的IFrame还没有加�?
                if (!panel['f_iframe_loaded']) {
                    window.setTimeout(function () {
                        // 如果此Panel已经创建完毕，但有时Panel可能是延迟创建的（比如TabStrip中的Tab，只有点击这个Tab时才创建Tab的内容）
                        panel['f_iframe_loaded'] = true;
                        panel.update(_createIFrameHtml(panel['f_iframe_url'], panel['f_iframe_name']));
                    }, 0);
                }
                else {
                    if (iframeUrlChanged) {
                        panel.body.query('iframe')[0].src = panel['f_iframe_url'];
                    }
                }
            }
        },


        // 处理表单中有任何字段发生变化时，关闭当前窗口时的提示
        confirmModified: function (closeFn) {
            if (F.util.isPageStateChanged()) {
                F.util.confirm('_self', F.wnd.formModifiedConfirmTitle, F.wnd.formChangeConfirmMsg, function () {
                    closeFn.apply(window, arguments);
                });
            } else {
                closeFn.apply(window, arguments);
            }
        },


        // Ext-Window中IFrame里页面中的表单发生变化时弹出确认消息
        iframeModifiedConfirm: function (panel, closeFn) {
            // 这个页面所在的Window对象
            var pageWindow = F.wnd.getIFrameWindowObject(panel);
            // 如果弹出的页面没能正常加载（比如说网络暂时连接中断）
            // 则直接关闭弹出的Ext-Window，而不会去检查页面表单变化，因为页面对象不存�?
            if (pageWindow.F) {
                pageWindow.F.wnd.confirmModified(closeFn);
            }
            else {
                panel.f_hide();
            }
        },

        // 取得Ghost Panel所在页面window对象
        getIFrameWindowObject: function (panel) {
            // 当前页面在IFrame中（也即�?window.frameElement 存在�?
            // 此Ext-Window需要在父窗口中弹出
            /*
            if (window.frameElement && panel['f_property_show_in_parent']) {
                panel = parent.F[panel['f_property_guid']];
            }
            */
            panel = F.wnd.getGhostPanel(panel);
            var iframeNode = Ext.query('iframe', panel.body.dom);
            if (iframeNode.length === 0) {
                // 当前panel（Ext-Window）不包含iframe
                return window;
            }
            else {
                return iframeNode[0].contentWindow;
            }
        },

        // 返回当前活动Window组件对象（浏览器窗口对象通过F.wnd.getActiveWindow().window获取�?
        getActiveWindow: function () {

            // Ext.WindowManager.getActive();有可能返回一个弹出对话框
            function getActiveFineUIWindow(wnd) {
                var result = wnd.Ext.WindowManager.getActive();
                if (!result.f_property_guid) {
                    wnd.Ext.WindowManager.eachTopDown(function (cmp) {
                        if (cmp.f_property_guid) {
                            result = cmp;
                            return false;
                        }
                    });
                }
                return result;
            }

            var activeWindow = parent.window;
            var activeExtWindow = getActiveFineUIWindow(activeWindow);
            if (activeExtWindow) {
                if (activeExtWindow['f_property_window']) {
                    activeWindow = activeExtWindow['f_property_window'];
                    activeExtWindow = activeExtWindow['f_property_ext_window'];
                }
                activeExtWindow.window = activeWindow;
            }
            return activeExtWindow;
        },


        // 向弹出此Ext-Window的页面写入�?
        writeBackValue: function () {
            var aw = F.wnd.getActiveWindow();
            var controlIds = aw['f_property_save_state_control_client_ids'];
            var controlCount = Math.min(controlIds.length, arguments.length);
            for (var i = 0; i < controlCount; i++) {
                aw.window.Ext.getCmp(controlIds[i]).setValue(arguments[i]);
            }
        }

    };

})();
�?
F.originalComponentHide = Ext.Component.prototype.hide;
Ext.override(Ext.Component, {

    // override
    hide: function () {
        var me = this;

        if (me.tab && me.tab.isXType('tab')) {
            // tabpanel 单独处理
        } else {
            // 除了 tabpanel 的其他面�?
            if (me.body) {
                // 检查当前组件内的表单是否改变（包含组件�?iframe 页面，递归查找所�?iframe�?
                if (F.util.preventPageClose(me.body)) {
                    return false;
                }
            }
        }

        return F.originalComponentHide.apply(me, arguments);
    },

    f_setDisabled: function () {
        this.setDisabled(!this.f_state['Enabled']);
    },

    f_setVisible: function () {
        this.setVisible(!this.f_state['Hidden']);
    },

    f_setWidth: function () {
        this.setWidth(this.f_state['Width']);
    },

    f_setHeight: function () {
        this.setHeight(this.f_state['Height']);
    }




});

// 1. tabpanel 单独处理，选项卡右上角的关闭按�?
F.originalTabBarCloseTab = Ext.tab.Bar.prototype.closeTab;
Ext.override(Ext.tab.Bar, {
    // override
    closeTab: function (toClose) {
        var me = this, card = toClose.card;

        if (card.body) {
            // 检查当前组件内的表单是否改变（包含组件�?iframe 页面，递归查找所�?iframe�?
            if (F.util.preventPageClose(card.body)) {
                return false;
            }
        }

        return F.originalTabBarCloseTab.apply(me, arguments);
    }

});

// 2. tabpanel 单独处理，选项卡的右键菜单
F.originalTabPanelRemove = Ext.tab.Panel.prototype.remove;
Ext.override(Ext.tab.Panel, {

    // override
    remove: function (comp) {
        var me = this, c = me.getComponent(comp);

        if (c && c.body) {
            // 检查当前组件内的表单是否改变（包含组件�?iframe 页面，递归查找所�?iframe�?
            if (F.util.preventPageClose(c.body)) {
                return false;
            }
        }

        return F.originalTabPanelRemove.apply(me, arguments);
    }

});


// 验证一个表单是否有效，会递归查询表单中每个字�?
// 如果表单隐藏或者字段隐藏，则不进行有效性校�?
Ext.override(Ext.container.Container, {

    f_isValid: function () {
        var valid = true;
        var firstInvalidField = null;
        if (!this.hidden) {
            this.items.each(function (f) {
                if (!f.hidden) {
                    if (f.isXType('field') || f.isXType('checkboxgroup')) {
                        if (!f.validate()) {
                            valid = false;
                            if (firstInvalidField == null) {
                                firstInvalidField = f;
                            }
                        }
                    } else if (f.isXType('container') && f.items.length) {
                        var validResult = f.f_isValid();
                        if (!validResult[0]) {
                            valid = false;
                            if (firstInvalidField == null) {
                                firstInvalidField = validResult[1];
                            }
                        }
                    }
                }
            });
        }
        return [valid, firstInvalidField];
    },

    f_reset: function () {
        var me = this;
        if (me.items && me.items.length) {
            me.items.each(function (item) {
                if (item.isXType('field')) {
                    item.reset();
                } else if (item.isXType('container') && item.items.length) {
                    item.f_reset();
                }
            });
        }
    },

    // 当前面板内的表单字段是否改变
    f_isDirty: function () {
        var me = this, dirty = false;

        if (me.items && me.items.length) {
            me.items.each(function (item) {
                if (item.isXType('field')) {
                    if (item.isDirty()) {
                        dirty = true;
                        return false;
                    }
                } else if (item.isXType('container') && item.items.length) {
                    if (item.f_isDirty()) {
                        dirty = true;
                        return false;
                    }
                }
            });
        }

        return dirty;
    },


    // 当前面板内的表单字段
    f_clearDirty: function () {
        var me = this;

        if (me.items && me.items.length) {
            me.items.each(function (item) {
                if (item.isXType('field')) {
                    item.resetOriginalValue();
                } else if (item.isXType('container') && item.items.length) {
                    item.f_clearDirty()
                }
            });
        }
    }

});

//F.originalPanelClose = Ext.panel.Panel.prototype.close;

Ext.override(Ext.panel.Panel, {

    //// override
    //close: function () {

    //    // 检查当前组件内的表单是否改变（包含组件�?iframe 页面，递归查找所�?iframe�?
    //    if (F.util.preventPageClose(this.body)) {
    //        return false;
    //    }


    //    return F.originalPanelClose.apply(this, arguments);
    //},


    f_setCollapse: function () {
        var collapsed = this.f_state['Collapsed'];
        if (collapsed) {
            this.collapse();
        } else {
            this.expand();
        }
    },

    f_isCollapsed: function () {
        /*
        var collapsed = false;
        var state = this.getState();
        if (state && state.collapsed) {
            collapsed = true;
        }
        return collapsed;
        */
        return !!this.getCollapsed();
    },

    f_setTitle: function () {
        this.setTitle(this.f_state['Title']);
    },

    f_getActiveIndex: function () {
        var activeIndex = -1;
        this.items.each(function (item, index) {
            if (item.f_isCollapsed && !item.f_isCollapsed()) {
                activeIndex = index;
                return false;
            }
        });
        return activeIndex;
    }


});

Ext.override(Ext.form.FieldSet, {
    f_setCollapse: function () {
        var collapsed = this.f_state['Collapsed'];
        if (collapsed) {
            this.collapse();
        } else {
            this.expand();
        }
    },

    f_isCollapsed: function () {
        /*
        var collapsed = false;
        var state = this.getState();
        if (state && state.collapsed) {
            collapsed = true;
        }
        return collapsed;
        */
        return !!this.getCollapsed();
    },

    f_setTitle: function () {
        this.setTitle(this.f_state['Title']);
    }

});

if (Ext.menu.CheckItem) {
    Ext.override(Ext.menu.CheckItem, {

        f_setChecked: function () {
            this.setChecked(this.f_state['Checked'], true);
        }

    });
}

if (Ext.form.field.Base) {
    Ext.override(Ext.form.field.Base, {

        //  Add functionality to Field's initComponent to enable the change event to bubble
        /*
        initComponent: Ext.form.Field.prototype.initComponent.createSequence(function () {
            this.enableBubble('change');
        }),
        */

        /* 这会导致在文本输入框中按回车键，无法触发type=submit的表单回发事�?
        listeners: {
        specialkey: function (field, e) {
        if (e.getKey() == e.ENTER) {
        e.stopEvent();
        }
        }
        },
        */

        /*
        // When show or hide the field, also hide the label.
        hide: function () {
        Ext.form.Field.superclass.hide.call(this);
        //this.callOverridden();

        //var label = Ext.get(this.el.findParent('div[class=x-form-item]')).first('label[for=' + this.id + ']');
        var labelAndField = this.el.findParentNode('div[class*=x-form-item]', 10, true);
        if (labelAndField) {
        if (this.hideMode == 'display') {
        labelAndField.setVisibilityMode(Ext.Element.DISPLAY);
        } else {
        labelAndField.setVisibilityMode(Ext.Element.VISIBILITY);
        }
        labelAndField.hide();
        }
        },

        show: function () {
        Ext.form.Field.superclass.show.call(this);
        //this.callOverridden();

        //var label = Ext.get(this.el.findParent('div[class=x-form-item]')).first('label[for=' + this.id + ']');
        var labelAndField = this.el.findParentNode('div[class*=x-form-item]', 10, true);
        if (labelAndField) {
        if (this.hideMode == 'display') {
        labelAndField.setVisibilityMode(Ext.Element.DISPLAY);
        } else {
        labelAndField.setVisibilityMode(Ext.Element.VISIBILITY);
        }
        labelAndField.show();
        }
        },
        */

        f_setValue: function (value) {
            if (typeof (value) === 'undefined') {
                value = this.f_state['Text'];
            }
            this.setValue(value);
        },

        f_setLabel: function (text) {
            /*
            if (this.label && this.label.update) {
                this.label.update(text || this.f_state['Label']);
            }
			*/
            var text = text || this.f_state['Label'];
            if (this.setFieldLabel) {
                this.setFieldLabel(text);
            }
        }

    });
}

if (Ext.form.field.Time) {
    Ext.override(Ext.form.field.Time, {

        // Time 继承�?ComboBox，这个函数被覆盖了，因此需要重新定�?
        f_setValue: function (value) {
            if (typeof (value) === 'undefined') {
                value = this.f_state['Text'];
            }
            this.setValue(value);
        }

    });
}


if (Ext.form.field.HtmlEditor) {
    Ext.override(Ext.form.field.HtmlEditor, {

        f_setValue: function (text) {
            if (typeof (text) === 'undefined') {
                text = this.f_state['Text'];
            }
            this.setValue(text);
        }

    });
}


if (Ext.form.field.Checkbox) {
    Ext.override(Ext.form.field.Checkbox, {

        f_setValue: function () {
            this.setValue(this.f_state['Checked']);
        }

    });
}


if (Ext.form.RadioGroup) {
    Ext.override(Ext.form.RadioGroup, {

        f_setValue: function (value) {
            value = value || this.f_state['SelectedValue'];
            var selectedObj = {};
            selectedObj[this.name] = value;
            this.setValue(selectedObj);
            //Ext.form.CheckboxGroup.prototype.f_setValue.apply(this, [value]);
        }

    });
}


if (Ext.form.CheckboxGroup) {
    Ext.override(Ext.form.CheckboxGroup, {

        f_reloadData: function (name, isradiogroup) {
            var container = this.ownerCt;
            var newConfig = Ext.apply(this.initialConfig, {
                "f_state": this.f_state,
                "items": F.util.resolveCheckBoxGroup(name, this.f_state, isradiogroup)
            });

            if (container) {
                var originalIndex = container.items.indexOf(this);
                container.remove(this, true);

                if (isradiogroup) {
                    container.insert(originalIndex, Ext.create('Ext.form.RadioGroup', newConfig));
                } else {
                    container.insert(originalIndex, Ext.create('Ext.form.CheckboxGroup', newConfig));
                }
                container.doLayout();
            } else {
                this.destroy();
                if (isradiogroup) {
                    Ext.create('Ext.form.RadioGroup', newConfig);
                } else {
                    Ext.create('Ext.form.CheckboxGroup', newConfig);
                }

            }
        },

        /*
        f_toBeDeleted: function () {
            var tobedeleted = this.items.items[0];
            if (tobedeleted && tobedeleted.inputValue === 'tobedeleted') {
                tobedeleted.destroy();
                this.items.remove(tobedeleted);
            }
        },
        */

        // 选中�?
        f_setValue: function (values) {
            // valueArray：["value1", "value2", "value3"]
            var values = values || this.f_state['SelectedValueArray'];

            var selectedObj = {};
            this.items.each(function (item) {
                var itemSelected = false;
                if (Ext.Array.indexOf(values, item.inputValue) >= 0) {
                    itemSelected = true;
                }
                selectedObj[item.name] = itemSelected;
            });

            this.setValue(selectedObj);
        },

        // 返回 ["value1", "value2", "value3"]
        f_getSelectedValues: function () {
            var selectedValues = [];
            var values = this.getValue();
            Ext.Object.each(values, function (key, value) {
                selectedValues.push(value);
            });
            return selectedValues;
        }

    });
}

if (Ext.form.field.ComboBox) {
    Ext.override(Ext.form.field.ComboBox, {
        // Load data from local cache.
        //        mode: "local",
        //        triggerAction: "all",
        displayField: "text",
        valueField: "value",
        //tpl: "<tpl for=\".\"><div class=\"x-combo-list-item <tpl if=\"!enabled\">x-combo-list-item-disable</tpl>\">{prefix}{text}</div></tpl>",

        // These variables are in the Ext.form.ComboBox.prototype, therefore all instance will refer to the same store instance.
        //store: new Ext.data.ArrayStore({ fields: ['value', 'text', 'enabled', 'prefix'] }),

        f_setValue: function (value) {
            // value 可以是空字符�?
            if (typeof (value) === 'undefined') {
                if (this.multiSelect) {
                    value = this.f_state['SelectedValueArray'];
                } else {
                    value = this.f_state['SelectedValue'];
                }
                //value = this.f_state['SelectedValue'];
            }
            this.setValue(value);
        },

        f_loadData: function (data) {
            data = data || this.f_state['F_Items'];
            if (data) {
                this.store.loadData(F.simulateTree.transform(data));
            }
        },


        f_getTextByValue: function (value, data) {
            data = data || this.f_state['F_Items'];
            value += ''; // 把Value转换为字符串
            for (var i = 0, count = data.length; i < count; i++) {
                var item = data[i];
                if (item[0] === value) {
                    return item[1];
                }
            }
            return '';
        }

    });
}


if (Ext.button.Button) {
    Ext.override(Ext.button.Button, {

        f_setTooltip: function () {
            this.setTooltip(this.f_state['ToolTip']);
        },

        f_toggle: function () {
            this.toggle(this.f_state['Pressed']);
        },

        f_setText: function () {
            this.setText(this.f_state['Text']);
        }


    });
}


if (Ext.grid.column.RowNumberer) {

    F.originalRowNumbererRenderer = Ext.grid.column.RowNumberer.prototype.renderer;
    Ext.override(Ext.grid.column.RowNumberer, {

        renderer: function () {

            var number = F.originalRowNumbererRenderer.apply(this, arguments);

            if (this.f_paging) {
                var pagingBar = F(this.f_paging_grid).f_getPaging();
                if (pagingBar) {
                    number += pagingBar.f_pageIndex * pagingBar.f_pageSize;
                }
            }

            return number;
        }
    });

}

if (Ext.grid.Panel) {
    Ext.override(Ext.grid.Panel, {

        f_getData: function () {
            var $this = this, data = this.f_state['F_Rows']['Values'];

            //////////////////////////////////////////////////
            var tpls = this.f_getTpls(this.f_tpls);

            // 将Grid1_ctl37与对应的outHTML放在哈希表中
            var tplsHash = {};
            var e = document.createElement('div');
            e.innerHTML = tpls;
            Ext.Array.each(e.childNodes, function (item, index) {
                tplsHash[item.id] = item.outerHTML.replace(/\r?\n\s*/ig, '');
            });

            /*
            // 将服务器返回的字符串转换为实际的JavaScript类型
            function resolveActualDataItem(fieldValue, fieldIndex) {
                var fieldType = $this.f_fields[fieldIndex].type;
                if (fieldType) {
                    if (fieldType === 'date') {
                        fieldValue = new Date(fieldValue);
                    } else if (fieldType === 'boolean') {
                        if (fieldValue == 'true' || fieldValue == '1') {
                            fieldValue = true;
                        } else {
                            fieldValue = false;
                        }
                    } else if (fieldType === 'float') {
                        fieldValue = parseFloat(fieldValue);
                    } else if (fieldType === 'int') {
                        fieldValue = parseInt(fieldValue, 10);
                    }
                }
                return fieldValue;
            }
            */

            // 不要改变 F_Rows.Values 的原始数据，因为这个值会被POST到后�?
            var newdata = [], newdataitem;
            Ext.Array.each(data, function (row, rowIndex) {
                newdataitem = [];
                Ext.Array.each(row, function (item, index) {
                    if (typeof (item) === 'string' && item.substr(0, 7) === "#@TPL@#") {
                        var clientId = $this.id + '_' + item.substr(7);
                        newdataitem.push('<div id="' + clientId + '_container">' + tplsHash[clientId] + '</div>');
                    } else {
                        //newdataitem.push(resolveActualDataItem(item, index));
                        newdataitem.push(item);
                    }
                });
                newdata.push(newdataitem);
            });
            //////////////////////////////////////////////////

            return newdata;
        },

        f_getTpls: function (paramTpls) {
            var tpls;
            if (typeof (paramTpls) !== 'undefined') {
                // 1. 如果Tpls存在于函数参数中
                tpls = paramTpls;
                this['data-last-tpls'] = tpls;
            } else {
                var tplsNode = Ext.get(this.id + '_tpls');
                if (tplsNode) {
                    // 2. 如果Tpls存在于页面节点中
                    tpls = tplsNode.dom.innerHTML;
                    // 获取模板列的内容之后，必须要删除原有的节点，因为会在表格中创建完全相同的新节�?
                    tplsNode.remove();

                    // 将模板列内容保存到表格实例中
                    this['data-last-tpls'] = tpls;
                } else {
                    // 3. 从缓存中读取
                    // 从表格实例中读取模板列内�?
                    tpls = this['data-last-tpls'];
                }
            }

            return tpls;
        },


        f_updateTpls: function (tpls) {
            tpls = this.f_getTpls(tpls);

            var e = document.createElement('div');
            e.innerHTML = tpls;
            Ext.Array.each(e.childNodes, function (item, index) {
                var nodeId = item.id;
                var tplContainer = Ext.get(nodeId + '_container');

                // 对于内存分页，模板列的内容可能还没有渲染到页面中
                if (tplContainer) {
                    tplContainer.dom.innerHTML = item.outerHTML;
                }
            });
        },

        f_getPaging: function () {
            var toolbar = this.getDockedItems('toolbar[dock="bottom"][xtype="simplepagingtoolbar"]');
            return toolbar.length ? toolbar[0] : undefined;
        },

        f_loadData: function () {
            var datas = this.f_getData();
            var pagingBar = this.f_getPaging();
            if (pagingBar) {
                var pagingDatas = [];
                if (pagingBar.f_databasePaging) {
                    pagingDatas = datas;
                } else {
                    for (var i = pagingBar.f_startRowIndex; i <= pagingBar.f_endRowIndex; i++) {
                        pagingDatas.push(datas[i]);
                    }
                }
                datas = pagingDatas;
            }


            var store = this.getStore();

            // 已经设置 Ext.data.Store �?pruneModifiedRecords ，在重新加载数据时都会清除所有已经改变的数据
            // 所以无需 rejectChanges
            // 拒绝之前对表格的编辑，因为接下来就要重新加载数据
            //store.rejectChanges();

            // 重新加载数据前清空之前的改变
            //this.f_newAddedRows = [];
            //this.f_deletedRows = [];

            store.loadData(datas);


            if (this.f_cellEditing) {
                store.commitChanges();
                this.f_initRecordIDs();
            }
        },

        // 初始化所有记录的ID列表
        f_initRecordIDs: function () {
            var $this = this;
            this.f_recordIDs = [];
            this.getStore().each(function (record, index) {
                $this.f_recordIDs.push(record.id);
            });
        },

        // 展开所有的行扩展列
        f_expandAllRows: function () {
            var expander = this.getPlugin(this.id + '_rowexpander');
            if (expander) {
                var store = this.getStore();
                for (var i = 0, count = store.getCount() ; i < count; i++) {
                    var record = store.getAt(i);
                    if (!expander.recordsExpanded[record.internalId]) {
                        expander.toggleRow(i, record);
                    }
                }
            }
        },

        // 隐藏所有的行扩展列
        f_collapseAllRows: function () {
            var expander = this.getPlugin(this.id + '_rowexpander');
            if (expander) {
                var store = this.getStore();
                for (var i = 0, count = store.getCount() ; i < count; i++) {
                    var record = store.getAt(i);
                    if (expander.recordsExpanded[record.internalId]) {
                        expander.toggleRow(i, record);
                    }
                }
            }
        },

        // http://evilcroco.name/2010/10/making-extjs-grid-content-selectable/
        // IE下允许选中表格中的文本
        /*
        f_enableTextSelection: function () {
            var elems = Ext.DomQuery.select("div[unselectable=on]", this.el.dom);
            for (var i = 0, len = elems.length; i < len; i++) {
                Ext.get(elems[i]).set({ 'unselectable': 'off' }).removeCls('x-unselectable');
            }
        },
        */

        // 获取选中的行数，或者单元格数（单元格编辑模式）
        f_getSelectedCount: function () {
            var selectedCount = 0;
            var sm = this.getSelectionModel();
            if (sm.hasSelection()) {
                if (sm.getCount) {
                    selectedCount = sm.getCount();
                } else {
                    // 单元格编辑模式，只可能选中一个单元格
                    selectedCount = 1;
                }
            }
            return selectedCount;
        },

        // 选中某些�?
        f_selectRows: function (rows) {
            rows = rows || this.f_state['SelectedRowIndexArray'] || [];
            var sm = this.getSelectionModel();
            if (sm.select) {
                sm.deselectAll(true);
                Ext.Array.each(rows, function (row, index) {
                    // select( records, [keepExisting], [suppressEvent] )
                    sm.select(row, true, true);
                });
            }
        },

        // 选中全部�?
        f_selectAllRows: function () {
            var sm = this.getSelectionModel();
            if (sm.selectAll) {
                sm.selectAll(true);
            }
        },

        // 获取选中的行
        f_getSelectedRows: function () {
            var selectedRows = [];
            var sm = this.getSelectionModel();
            if (sm.getSelection) {
                var selection = sm.getSelection();
                var store = this.getStore();

                Ext.Array.each(selection, function (record, index) {
                    selectedRows.push(store.indexOf(record));
                });
            }

            return selectedRows;
        },


        // 选中单元格（AllowCellEditing�?
        f_selectCell: function (cell) {
            cell = cell || this.f_state['SelectedCell'] || [];
            var sm = this.getSelectionModel();
            if (sm.select) {
                if (cell.length === 2) {
                    sm.setCurrentPosition({
                        row: cell[0],
                        column: cell[1]
                    });
                } else {
                    // TODO:
                    //sm.deselectAll();
                }
            }
        },

        // 获取选中的单元格（AllowCellEditing�?
        f_getSelectedCell: function () {
            var selectedCell = [], currentPos;
            var sm = this.getSelectionModel();
            if (sm.getCurrentPosition) {
                currentPos = sm.getCurrentPosition();
                if (currentPos) {
                    selectedCell = [currentPos.row, currentPos.columnHeader.f_columnIndex];
                }
            }
            return selectedCell;
        },


        // 获取隐藏列的名称列表
        f_getHiddenColumns: function () {
            var hiddens = [], columns = this.f_getColumns();
            Ext.Array.each(columns, function (column, index) {
                var columnId = column.id;

                // 行扩展列需要单独处理，id属性不�?expander
                if (!column.dataIndex && column.innerCls && column.innerCls.indexOf('row-expander') > 0) {
                    columnId = 'expander';

                }

                if (column.isHidden()) {
                    hiddens.push(columnId);
                }
            });
            return hiddens;
        },

        // 隐藏需要隐藏的列，显示不需要隐藏的�?
        f_updateColumnsHiddenStatus: function (hiddens) {
            hiddens = hiddens || this.f_state['HiddenColumns'] || [];
            var columns = this.f_getColumns();
            Ext.Array.each(columns, function (column, index) {
                var columnId = column.id;

                // 行扩展列需要单独处理，id属性不�?expander
                if (!column.dataIndex && column.innerCls && column.innerCls.indexOf('row-expander') > 0) {
                    columnId = 'expander';
                }

                if (Ext.Array.indexOf(hiddens, columnId) !== -1) {
                    column.setVisible(false);
                } else {
                    column.setVisible(true);
                }
            });
        },

        // 初始化排序列�?
        f_initSortHeaders: function () {
            var gridEl = Ext.get(this.id), columns = this.f_getColumns();

            // 为所有可排序列添加手型光�?
            Ext.Array.each(columns, function (item, index) {
                if (item['sortable']) {
                    Ext.get(item.id).addCls('cursor-pointer');
                }
            });
        },

        // 设置表格标题栏的排序图标
        f_setSortIcon: function (sortColumnID, sortDirection) {
            var gridEl = Ext.get(this.id), columns = this.f_getColumns(), headers = gridEl.select('.x-column-header');

            // 清空所有可排序列的排序箭头
            headers.removeCls(['x-column-header-sort-DESC', 'x-column-header-sort-ASC']);

            // 为所有可排序列添加手型光�?
            Ext.Array.each(columns, function (item, index) {
                if (item['sortable']) {
                    Ext.get(item.id).addCls('cursor-pointer');
                }
            });

            // 设置当前列的排序箭头
            if (sortColumnID) {
                Ext.get(sortColumnID).addCls('x-column-header-sort-' + sortDirection.toUpperCase());
            }

        },

        // 获取表格�?
        f_getColumns: function () {
            /*
            var columns = [];
            var configColumns = this.getColumnModel().config;
            Ext.Array.each(configColumns, function (item, index) {
                // expander也属于表格列的一种类型，否则设置f_setSortIcon会出�?
                if (item.id !== 'numberer' && item.id !== 'checker') { // && item.id !== 'expander'
                    columns.push(item);
                }
            });
            */

            // columns 属性不包含行扩展列
            //return this.columns;

            // this.columnManager.columns 返回相同内容
            return this.headerCt.getGridColumns();
        },

        // 这个方法用不到了，现在对States的更新会导致Values的改变，进而促使表格的重新加载
        /*
        f_setRowStates: function (states) {
        var gridEl = Ext.get(this.id), columns = this.f_getColumns(), states = states || this.f_state['f_states'] || [];

        function setCheckBoxStates(columnIndex, stateColumnIndex) {
        var checkboxRows = gridEl.select('.x-grid-body .x-grid-row .x-grid-td-' + columns[columnIndex].id + ' .f-grid-checkbox');
        checkboxRows.each(function (row, rows, index) {
        if (states[index][stateColumnIndex]) {
        if (row.hasCls('box-grid-checkbox-unchecked-disabled')) {
        row.removeCls('box-grid-checkbox-unchecked-disabled');
        } else {
        row.removeCls('box-grid-checkbox-unchecked');
        }
        } else {
        if (row.hasCls('box-grid-checkbox-disabled')) {
        row.addCls('box-grid-checkbox-unchecked-disabled')
        } else {
        row.addCls('box-grid-checkbox-unchecked')
        }
        }
        });
        }

        var stateColumnIndex = 0;
        Ext.Array.each(columns, function (column, index) {
        if (column['f_persistState']) {
        if (column['f_persistStateType'] === 'checkbox') {
        setCheckBoxStates(index, stateColumnIndex);
        stateColumnIndex++;
        }
        }
        });
        },
        */

        // 获取列状态（目前只有CheckBoxField用到�?
        f_getStates: function () {
            var gridEl = Ext.get(this.id), columns = this.f_getColumns(), states = [];

            function getCheckBoxStates(columnIndex) {
                var checkboxRows = gridEl.select('.x-grid-row .x-grid-cell-' + columns[columnIndex].id + ' .f-grid-checkbox');
                var columnStates = [];
                checkboxRows.each(function (row, index) {
                    if (row.hasCls('unchecked')) {
                        columnStates.push(false);
                    } else {
                        columnStates.push(true);
                    }
                });
                return columnStates;
            }

            Ext.Array.each(columns, function (column, index) {
                if (column['f_persistState']) {
                    if (column['f_persistStateType'] === 'checkbox') {
                        states.push(getCheckBoxStates(index));
                    }
                }
            });

            // 把列状态列表转换为行状态列表，与后台数据保持一�?
            var i, resolvedStates = [], rowState, rowCount;
            if (states.length > 0) {
                rowCount = states[0].length;
                for (i = 0; i < rowCount; i++) {
                    rowState = [];
                    Ext.Array.each(states, function (state, index) {
                        rowState.push(state[i]);
                    });
                    resolvedStates.push(rowState);
                }
            }

            return resolvedStates;
        },

        // 提交客户端改�?
        f_commitChanges: function () {

            if (this.f_cellEditing) {
                this.getStore().commitChanges();
                this.f_initRecordIDs();
            }

        },


        // 从Store中删除选中的行（或者单元格�?
        f_deleteSelected: function () {
            var $this = this;
            var store = this.getStore();

            var sm = this.getSelectionModel();
            if (sm.getSelection) {
                var rows = this.f_getSelectedRows();
                Ext.Array.each(rows, function (rowIndex, index) {
                    store.removeAt(rowIndex);
                });
            } else if (sm.getSelectedCell) {
                var selectedCell = this.f_getSelectedCell();
                if (selectedCell.length) {
                    store.removeAt(selectedCell[0]);
                }
            }
        },

        // 添加一条新纪录
        f_addNewRecord: function (defaultObj, appendToEnd) {
            var i, count, store = this.getStore();
            var newRecord = defaultObj; //new Ext.data.Model(defaultObj);

            this.f_cellEditing.cancelEdit();

            var rowIndex = 0;
            if (appendToEnd) {
                store.add(newRecord);
                rowIndex = store.getCount() - 1;
            } else {
                store.insert(0, newRecord);
                rowIndex = 0;
            }
            this.f_cellEditing.startEditByPosition({
                row: rowIndex,
                column: this.f_firstEditableColumnIndex()
            });
        },

        //// 获取新增的行索引（在修改后的列表中）
        //f_getNewAddedRows: function () {
        //    var $this = this;
        //    var newAddedRows = [];
        //    this.getStore().each(function (record, index) {
        //        if (Ext.Array.indexOf($this.f_recordIDs, record.id) < 0) {
        //            newAddedRows.push(index);
        //        }
        //    });
        //    return newAddedRows;
        //},

        // 获取删除的行索引（在原始的列表中�?
        f_getDeletedRows: function () {
            var currentRecordIDs = [], deletedRows = [];
            this.getStore().each(function (record, index) {
                currentRecordIDs.push(record.id);
            });

            // 快速判断是否存在行被删除的情况
            if (currentRecordIDs.join('') === this.f_recordIDs.join('')) {
                return deletedRows;
            }

            Ext.Array.each(this.f_recordIDs, function (recordID, index) {
                if (Ext.Array.indexOf(currentRecordIDs, recordID) < 0) {
                    deletedRows.push(index);
                }
            });
            return deletedRows;
        },

        f_firstEditableColumnIndex: function () {
            var i = 0, count = this.columns.length, column;
            for (; i < count; i++) {
                column = this.columns[i];
                if ((column.getEditor && column.getEditor()) || column.xtype === 'checkcolumn') {
                    return i;
                }
            }
            return 0;
        },

        f_columnEditable: function (columnID) {
            var i = 0, count = this.columns.length, column;
            for (; i < count; i++) {
                column = this.columns[i];
                if (column.id === columnID) {
                    if ((column.getEditor && column.getEditor()) || column.xtype === 'checkcolumn') {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            return false;
        },

        // 获取用户修改的单元格�?
        f_getModifiedData: function () {
            var me = this, i, j, count, columns = this.f_getColumns();

            /*
            Ext.Array.each(columns, function (column, index) {
                columnMap[column.id] = column;
            });

            function checkColumnEditable(columnID) {
                var column = columnMap[columnID];
                if (column && (column.hasEditor() || column.xtype === 'checkcolumn')) {
                    return true;
                }
                return false;
            }
            */

            var modifiedRows = [];
            var store = this.getStore();
            var modifiedRecords = store.getModifiedRecords();
            var rowIndex, rowData, newData, modifiedRecord, recordID, rowIndexOriginal;
            for (i = 0, count = modifiedRecords.length; i < count; i++) {
                modifiedRecord = modifiedRecords[i];
                recordID = modifiedRecord.id;
                rowIndex = store.indexOf(modifiedRecord);
                rowData = modifiedRecord.data;
                if (rowIndex < 0) {
                    continue;
                }

                // 本行数据在原始数据集合中的行索引
                rowIndexOriginal = Ext.Array.indexOf(this.f_recordIDs, recordID);
                if (rowIndexOriginal < 0) {
                    var newRowData = {};
                    // 删除那些不能编辑的列
                    for (var columnID in rowData) {
                        if (this.f_columnEditable(columnID)) {
                            //delete rowData[columnID];
                            var rowDataColumn = rowData[columnID];
                            // 如果是日期对象，则转化为字符�?
                            if (F.util.isDate(rowDataColumn)) {
                                rowDataColumn = F.util.resolveGridDateToString(me.f_fields, columnID, rowDataColumn);
                            }
                            newRowData[columnID] = rowDataColumn;
                        }
                    }
                    // 新增数据�?
                    modifiedRows.push([rowIndex, -1, newRowData]);
                } else {
                    var rowModifiedObj = {};
                    for (var columnID in modifiedRecord.modified) {
                        if (this.f_columnEditable(columnID)) {
                            newData = rowData[columnID];
                            // 如果是日期对象，则转化为字符�?
                            if (F.util.isDate(newData)) {
                                newData = F.util.resolveGridDateToString(me.f_fields, columnID, newData);
                            }
                            rowModifiedObj[columnID] = newData;
                        }
                    }
                    // 修改现有数据�?
                    modifiedRows.push([rowIndex, rowIndexOriginal, rowModifiedObj]);
                }
            }

            // 结果按照 rowIndex 升序排序
            return modifiedRows.sort(function (a, b) { return a[0] - b[0]; });
        }

    });
}


if (Ext.tree.Panel) {
    Ext.override(Ext.tree.Panel, {

        f_loadData: function () {
            var datas = this.f_state['F_Nodes'];
            var nodes = this.f_tranformData(datas);
            var root = this.getRootNode();
            if (root) {
                root.removeAll();
            }
            this.setRootNode({
                //id: this.id + '_root',
                expanded: true,
                children: nodes
            });
        },

        f_tranformData: function (datas) {
            var that = this, i = 0, nodes = [];
            for (i = 0; i < datas.length; i++) {
                var data = datas[i], node = {};

                // 0 - Text
                // 1 - Leaf
                // 2 - NodeID
                // 3 - Enabled
                // 4 - EnableCheckBox
                // 5 - Checked
                // 6 - Expanded
                // 7 - NavigateUrl
                // 8 - Target
                // 9 - href
                // 10 - Icon
                // 11 - IconUrl
                // 12 - iconUrl
                // 13 - ToolTip
                // 14 - OnClientClick
                // 15 - EnableClickEvent
                // 16 - CommandName
                // 17 - CommandArgument

                // 18 - EnableCheckEvent
                // 19 - EnableExpandEvent
                // 20 - EnableCollapseEvent

                // 21 - Nodes
                node.text = data[0];
                node.leaf = !!data[1];
                node.id = data[2];
                node.disabled = !data[3];
                if (!!data[4]) {
                    // node.checked === undefined, no checkbox
                    node.checked = !!data[5];
                }
                if (!data[1]) {
                    node.expanded = !!data[6];
                }
                if (data[9]) {
                    node.href = data[9];
                    node.hrefTarget = data[8];
                }
                if (data[12]) {
                    node.icon = data[12];
                }
                node.qtip = data[13];

                if (data[14]) {
                    node.f_clientclick = data[14];
                }
                node.f_enableclickevent = !!data[15];
                node.f_commandname = data[16];
                node.f_commandargument = data[17];

                node.f_enablecheckevent = !!data[18];

                node.f_enableexpandevent = !!data[19];
                node.f_enablecollapseevent = !!data[20];


                if (data[21] && data[21].length > 0) {
                    node.children = that.f_tranformData(data[21]);
                }

                nodes.push(node);
            }
            return nodes;
        },

        f_getExpandedNodes: function (nodes) {
            var i = 0, that = this, expandedNodes = [];

            for (; i < nodes.length; i++) {
                var node = nodes[i];
                if (node.isExpanded()) {
                    expandedNodes.push(node.getId());
                }
                if (node.hasChildNodes()) {
                    expandedNodes = expandedNodes.concat(that.f_getExpandedNodes(node.childNodes));
                }
            }

            return expandedNodes;
        },

        f_getCheckedNodes: function () {
            var checkedIDs = [], checkedArray = this.getChecked();
            Ext.Array.each(checkedArray, function (node, index) {
                checkedIDs.push(node.getId());
            });
            return checkedIDs;
        },

        f_getSelectedNodes: function () {
            var selectedNodeIDs = [];
            var sm = this.getSelectionModel();
            if (sm.getSelection) {
                var selection = sm.getSelection();

                Ext.Array.each(selection, function (node, index) {
                    selectedNodeIDs.push(node.getId());
                });
            }

            return selectedNodeIDs;
        },

        f_selectNodes: function () {
            var nodeIDs = this.f_state['SelectedNodeIDArray'] || [];
            var model = this.getSelectionModel(), store = this.getStore(), nodes = [], node;
            Ext.Array.each(nodeIDs, function (nodeID, index) {
                node = store.getNodeById(nodeID);
                if (node) {
                    nodes.push(node);
                }
            });
            model.deselectAll(true);
            model.select(nodes);
        }


    });
}


if (Ext.PagingToolbar) {
    // We don't use this Class in current version.
    Ext.override(Ext.PagingToolbar, {

        f_hideRefresh: function () {
            var index = this.items.indexOf(this.refresh);
            this.items.get(index - 1).hide();
            this.refresh.hide();
        }

    });
}


if (Ext.tab.Panel) {
    Ext.override(Ext.tab.Panel, {

        f_autoPostBackTabsContains: function (tabId) {
            var tabs = this.f_state['F_AutoPostBackTabs'];
            return tabs.indexOf(tabId) !== -1;
        },

        f_setActiveTab: function () {
            var tabIndex = this.f_state['ActiveTabIndex'];
            this.setActiveTab(tabIndex);
        },

        f_getActiveTabIndex: function () {
            return this.items.indexOf(this.getActiveTab());
        },

        /*
        activateNextTab: function (c) {
            if (c == this.activeTab) {
                var next = this.stack.next();
                if (next) {
                    this.setActiveTab(next);
                }
                if (next = this.items.find(function (t) { return t.tabEl.style.display !== 'none'; })) {
                    // Find the first visible tab and set it active tab. 
                    this.setActiveTab(next);
                } else {
                    this.setActiveTab(null);
                }
            }
        },
        */

        hideTab: function (tabId) {
            var tab = F(tabId).tab;
            if (tab) {
                tab.hide();
            }
        },

        showTab: function (tabId) {
            var tab = F(tabId).tab;
            if (tab) {
                tab.show();
            }
        },

        addTab: function (id, url, title, closable) {
            var options = {
                'cls': 'f-tab'
            }, tab;
            if (typeof (id) === 'string') {
                Ext.apply(options, {
                    'id': id,
                    'title': title,
                    'closable': closable,
                    'url': url
                });
            } else {
                // 如果id不是字符串，则id为对象并且只有一个参�?
                Ext.apply(options, id);
            }

            tab = this.getTab(options.id);
            if (!tab) {
                Ext.apply(options, {
                    'f_dynamic_added_tab': true,
                    'html': '<iframe id="' + options.id + '" name="' + options.id + '" src="' + options.url + '" frameborder="0" style="height:100%;width:100%;overflow:auto;"\></iframe\>'
                });
                tab = this.add(options);
            }

            this.setActiveTab(tab);

            return tab;
        },

        getTab: function (tabId) {
            return F(tabId);
        },

        removeTab: function (tabId) {
            this.remove(tabId);
        }

    });
}

if (Ext.WindowManager) {

    Ext.override(Ext.WindowManager, {

        // 确保窗体的遮罩层覆盖整个窗口
        getMaskBox: function () {
            this.mask.maskTarget = Ext.getBody();
            return this.callParent(arguments);
        }

    });
}

if (Ext.window.Window) {

    Ext.override(Ext.window.Window, {
        /*
        hide: function () {
            this.callParent(arguments);
            if (this.modal) {
                Ext.select('.x-mask').setStyle({ top: 0, left: 0, width: '100%', height: '100%' });
            }
        },

        show: function(){
            this.callParent(arguments);
            if (this.modal) {
                Ext.select('.x-mask').setStyle({ top: 0, left: 0, width: '100%', height: '100%' });
            }
        },
        */

        // @private
        onWindowResize: function () {
            var me = this;
            if (me.maximized) {
                // 改变浏览器大小可以自动调整窗体控件的大小（窗体控件最大化时）
                F.wnd.fixMaximize(me);
            } else {
                me.callParent();
            }
        },

        /*
        bof_hide: function () {
            this.f_hide();
        },
        bof_hide_refresh: function () {
            this.f_hide_refresh();
        },
        bof_hide_postback: function (argument) {
            this.f_hide_postback(argument);
        },
        bof_show: function (iframeUrl, windowTitle) {
            this.f_show(iframeUrl, windowTitle);
        },
        */

        f_setWidth: function () {
            var panel = F.wnd.getGhostPanel(this);
            panel.setWidth(this.f_state['Width']);
        },

        f_setHeight: function () {
            var panel = F.wnd.getGhostPanel(this);
            panel.setHeight(this.f_state['Height']);
        },

        f_setTitle: function () {
            var panel = F.wnd.getGhostPanel(this);
            panel.setTitle(this.f_state['Title']);
        },

        f_hide: function () {
            F.wnd.hide(this, this.f_iframe, this.id + '_Hidden');
        },
        f_hide_refresh: function () {
            this.f_hide();
            window.location.reload();
        },
        f_hide_postback: function (argument) {
            var me = this;
            me.f_hide();

            if (me.f_property_enable_ajax === false) {
                F.control_enable_ajax = false;
            }

            // 如果argument为undefined，则传入 __doPostBack �?argument 应该为空字符�?
            argument = argument || '';
            __doPostBack(me.name, 'Close$' + argument);
        },
        f_hide_executescript: function (scripts) {
            var me = this;
            me.f_hide();

            if (scripts) {
                with (window) {
                    new Function(scripts)();
                }
            }
        },
        f_show: function (iframeUrl, windowTitle, width, height) {
            var me = this;
            if (typeof (iframeUrl) === 'undefined') {
                iframeUrl = me.f_iframe_url;
            }
            if (typeof (windowTitle) === 'undefined') {
                windowTitle = me.title;
            }
            F.wnd.show(me, iframeUrl, windowTitle, me.f_property_left, me.f_property_top, me.f_property_position, me.id + '_Hidden', width, height);
        },

        f_maximize: function () {
            F.wnd.maximize(this);
        },
        f_minimize: function () {
            F.wnd.minimize(this);
        },
        f_restore: function () {
            F.wnd.restore(this);
        }



    });
}



if (Ext.grid.plugin.RowExpander) {
    Ext.override(Ext.grid.plugin.RowExpander, {

        // 将行扩展列的 td CSS类改�?x-grid-cell-row-expander
        getHeaderConfig: function () {
            var config = this.callParent(arguments);
            config.tdCls = Ext.baseCSSPrefix + 'grid-cell-row-expander';
            return config;
        }

    });
}

// 修正IE7下，窗口出现滚动条时，点击Window控件标题栏有时node为null的问�?
/*
if (Ext.dd.DragDrop) {
    F.originalIsValidHandleChild = Ext.dd.DragDrop.prototype.isValidHandleChild;
    Ext.dd.DragDrop.prototype.isValidHandleChild = function (node) {
        if (!node || !node.nodeName) {
            return false;
        }
        return F.originalIsValidHandleChild.apply(this, [node]);
    };
}
*/


// 修正在IE下，Grid的模版列中出现文本输入框或者下拉列表时，第一次不能选中的问�?
// 已经有网友发现这个问题：http://www.sencha.com/forum/archive/index.php/t-49653.html
// This is what caused my self-rendered-Html-Elements to "flicker" as described in my other thread. 
// The Dropdown receives the Click, opens and stays open for the Millisecond until
// Ext calls back and gives focus to the Cell, causing my Drop-Down to close again.
/*
if (Ext.grid.GridPanel) {
    Ext.grid.GridView.prototype.focusCell = function (row, col, hscroll) {
        this.syncFocusEl(this.ensureVisible(row, col, hscroll));

        var focusEl = this.focusEl;

        focusEl.focus();
    };
}
*/

// 修正Chrome下多表头样式错位
// 增加 !Ext.isChrome 的判断，在Chrome下DIV的宽度不包括边框的宽�?
/*
if (Ext.ux.grid && Ext.ux.grid.ColumnHeaderGroup) {
    Ext.ux.grid.ColumnHeaderGroup.prototype.getGroupStyle = function (group, gcol) {
        var width = 0, hidden = true;
        for (var i = gcol, len = gcol + group.colspan; i < len; i++) {
            if (!this.cm.isHidden(i)) {
                var cw = this.cm.getColumnWidth(i);
                if (typeof cw == 'number') {
                    width += cw;
                }
                hidden = false;
            }
        }
        return {
            width: (Ext.isBorderBox || (Ext.isWebKit && !Ext.isSafari2 && !Ext.isChrome) ? width : Math.max(width - this.borderWidth, 0)) + 'px',
            hidden: hidden
        };
    };
}
*/



// 修正IE7/IE8下Date.parse('2015-10-01')出错的问�?
// http://jibbering.com/faq/#parseDate
(function () {
    function parseISO8601(dateStr) {
        var isoExp = /(\d{2,4})-(\d\d?)-(\d\d?)/,
       date = new Date(NaN), month,
       parts = isoExp.exec(dateStr);

        if (parts) {
            month = +parts[2];
            date.setFullYear(parts[1], month - 1, parts[3]);
            if (month != date.getMonth() + 1) {
                date.setTime(NaN);
            }
        }
        return date;
    }

    var originalParse = Date.parse;
    Date.parse = function (dateStr) {
        var date = originalParse(dateStr);
        if (isNaN(date)) {
            date = parseISO8601(dateStr);
        }
        return date;
    }

})();



if (Ext.form.field.ComboBox) {
    F.originalComboSetValue = Ext.form.field.ComboBox.prototype.setValue;
    Ext.form.field.ComboBox.prototype.setValue = function (value, doSelect) {
        // value可能是数字（可编辑单元格，列的FieldType可能是Int�?
        if (typeof (value) === 'number' || typeof (value) === 'boolean') {
            value += '';
        }
        return F.originalComboSetValue.apply(this, [value, doSelect]);
    };
}

�?
(function() {

    function getParentIndex(levels, level, index) {
        if (level > 0) {
            for (var i = index - 1; i >= 0; i--) {
                if (levels[i] == level - 1) {
                    return i;
                }
            }
        }
        return -1;
    }

    function hasLittleBrother(levels, level, index) {
        if (index < levels.length - 1) {
            for (var i = index + 1; i < levels.length; i++) {
                if (levels[i] == level) {
                    return true;
                } else if (levels[i] < level) {
                    return false;
                }
            }
        }
        return false;
    }

    function getParentTempData(tempdatas, tempdata, prefixIndex) {
        for (var i = 0; i < prefixIndex - 1; i++) {
            tempdata = tempdatas[tempdata.parentIndex];
        }
        return tempdata;
    }

    function getPrefixInner(tempdatas, tempdata, prefixIndex) {
        // If level = 3, then prefixIndex array will be: [3, 2, 1]
        // prefixIndex === 1 will always present the nearest prefix next to the Text.
        if (prefixIndex === 1) {
            if (tempdata.littleBrother) {
                return '<div class="x-elbow"></div>';
            }
            else {
                return '<div class="x-elbow-end"></div>';
            }
        } else {
            var parentdata = getParentTempData(tempdatas, tempdata, prefixIndex);
            if (parentdata.littleBrother) {
                return '<div class="x-elbow-line"></div>';
            }
            else {
                return '<div class="x-elbow-empty"></div>';
            }
        }
        return "";
    }

    function getPrefix(tempdatas, index) {
        var tempdata = tempdatas[index];
        var level = tempdata.level;
        var prefix = [];
        for (var i = level; i > 0; i--) {
            prefix.push(getPrefixInner(tempdatas, tempdata, i));
        }
        return prefix.join('');
    }

    F.simulateTree = {

        transform: function(datas) {
            if (!datas.length || datas[0].length < 4) {
                return datas;
            }

            //// store: new Ext.data.ArrayStore({ fields: ['value', 'text', 'enabled', 'prefix'] })
            //// Sample data:      
            //[
            //    ["0", "jQuery", 0, 0],
            //    ["1", "Core", 0, 1],
            //    ["2", "Selectors", 0, 1],
            //    ["3", "Basic Filters", 1, 2],
            //    ["4", "Content Filters", 1, 2],
            //    ["41", "Contains", 1, 3],
            //    ["5", "Attribute Filters", 1, 2],
            //    ["6", "Traversing", 1, 1],
            //    ["7", "Filtering", 1, 2],
            //    ["8", "Finding", 1, 2],
            //    ["9", "Events", 0, 1],
            //    ["10", "Page Load", 1, 2],
            //    ["11", "Event Handling", 1, 2],
            //    ["12", "Interaction Helpers", 1, 2],
            //    ["13", "Ajax", 1, 1]
            //]
            var levels = [];
            Ext.Array.each(datas, function (data, index) {
                levels.push(data[3]);
            });

            var tempdatas = [];
            Ext.Array.each(levels, function (level, index) {
                tempdatas.push({
                    'level': level,
                    'parentIndex': getParentIndex(levels, level, index),
                    'littleBrother': hasLittleBrother(levels, level, index)
                });
            });

            var newdatas = [];
            Ext.Array.each(datas, function (data, index) {
                newdatas.push([data[0], data[1], data[2], getPrefix(tempdatas, index)]);
            });
            return newdatas;

        }


    };

})();�?
(function () {

    var ExtF = Ext.util.Format;

    F.format = {

        capitalize: ExtF.capitalize,

        dateRenderer: ExtF.dateRenderer,

        ellipsisRenderer: function (length) {
            return function (value) {
                return ExtF.ellipsis(value, length, false);
            };
        },

        fileSize: ExtF.fileSize,

        htmlEncode: ExtF.htmlEncode,

        htmlDecode: ExtF.htmlDecode,

        lowercase: ExtF.lowercase,

        uppercase: ExtF.uppercase,

        nl2br: ExtF.nl2br,

        //number: ExtF.numberRenderer,

        stripScripts: ExtF.stripScripts,

        stripTags: ExtF.stripTags,

        trim: ExtF.trim

        //usMoney: ExtF.usMoney



    };


})();
Ext.define('Ext.ux.FormViewport', {
    extend: 'Ext.container.Container',
    alias: 'widget.formviewport',

    isViewport: true,

    ariaRole: 'application',

    preserveElOnDestroy: true,

    viewportCls: Ext.baseCSSPrefix + 'viewport',

    initComponent: function () {
        var me = this,
            html = document.body.parentNode,
            el = me.el = Ext.getBody();

        /////��ʼ �޸ĵ�һ/////////////////////////////
        el = me.el = Ext.get(me.renderTo);
        var body = Ext.getBody();
        /////���� �޸ĵ�һ/////////////////////////////


        // Get the DOM disruption over with before the Viewport renders and begins a layout
        Ext.getScrollbarSize();

        // Clear any dimensions, we will size later on
        me.width = me.height = undefined;

        me.callParent(arguments);
        Ext.fly(html).addCls(me.viewportCls);
        if (me.autoScroll) {
            Ext.fly(html).setStyle(me.getOverflowStyle());
            delete me.autoScroll;
        }
        el.setHeight = el.setWidth = Ext.emptyFn;
        el.dom.scroll = 'no';
        me.allowDomMove = false;
        me.renderTo = me.el;

    },

    // override here to prevent an extraneous warning
    applyTargetCls: function (targetCls) {
        this.el.addCls(targetCls);
    },

    onRender: function () {
        var me = this;

        me.callParent(arguments);

        // Important to start life as the proper size (to avoid extra layouts)
        // But after render so that the size is not stamped into the body
        me.width = Ext.Element.getViewportWidth();
        me.height = Ext.Element.getViewportHeight();
    },

    afterFirstLayout: function () {
        var me = this;

        me.callParent(arguments);
        setTimeout(function () {
            Ext.EventManager.onWindowResize(me.fireResize, me);
        }, 1);
    },

    fireResize: function (width, height) {
        // In IE we can get resize events that have our current size, so we ignore them
        // to avoid the useless layout...
        if (width != this.width || height != this.height) {
            this.setSize(width, height);
        }
    },

    initHierarchyState: function (hierarchyState) {
        this.callParent([this.hierarchyState = Ext.rootHierarchyState]);
    },

    beforeDestroy: function () {
        var me = this;

        me.removeUIFromElement();
        me.el.removeCls(me.baseCls);
        Ext.fly(document.body.parentNode).removeCls(me.viewportCls);
        me.callParent();
    }

});


Ext.define('Ext.ux.SimplePagingToolbar', {
    extend: 'Ext.toolbar.Paging',
    alias: 'widget.simplepagingtoolbar',

    cls: 'x-toolbar-paging',

    // Override parent
    initComponent: function () {
        var me = this;

        me.store = Ext.Object.merge({}, me.store, {
            getCount: function () {
                return me.f_recordCount;
            },
            currentPage: me.f_pageIndex + 1
        });

        me.callParent();
    },

    // Override parent
    getPagingItems: function() {
        var items = this.callParent();
        // Remove refresh and separator items.
        return items.slice(0, items.length - 2);
    },

    // Override parent
    getPageData: function () {
        var fromRecord = 0, toRecord = 0;
        if (this.f_databasePaging) {
            fromRecord = (this.f_pageIndex * this.f_pageSize) + 1;
            toRecord = fromRecord + this.f_pageSize - 1;
        } else {
            fromRecord = this.f_startRowIndex + 1;
            toRecord = this.f_endRowIndex + 1;
        }
        if (toRecord > this.f_recordCount) {
            toRecord = this.f_recordCount;
        }

        return {
            total: this.f_recordCount,
            currentPage: this.f_pageIndex + 1,
            pageCount: this.f_pageCount <= 0 ? 1 : this.f_pageCount,
            fromRecord: fromRecord,
            toRecord: toRecord
        };
    },

    f_update: function (configs) {
        Ext.Object.merge(this, configs);
        this.store.currentPage = this.f_pageIndex + 1;
        this.onLoad();
    }

});
/**
 * Plugin for adding a close context menu to tabs. Note that the menu respects
 * the closable configuration on the tab. As such, commands like remove others
 * and remove all will not remove items that are not closable.
 */
Ext.define('Ext.ux.TabCloseMenu', {
    alias: 'plugin.tabclosemenu',

    mixins: {
        observable: 'Ext.util.Observable'
    },

    /**
     * @cfg {String} closeTabText
     * The text for closing the current tab.
     */
    closeTabText: 'Close Tab',

    /**
     * @cfg {Boolean} showCloseOthers
     * Indicates whether to show the 'Close Others' option.
     */
    showCloseOthers: true,

    /**
     * @cfg {String} closeOthersTabsText
     * The text for closing all tabs except the current one.
     */
    closeOthersTabsText: 'Close Other Tabs',

    /**
     * @cfg {Boolean} showCloseAll
     * Indicates whether to show the 'Close All' option.
     */
    showCloseAll: true,

    /**
     * @cfg {String} closeAllTabsText
     * The text for closing all tabs.
     */
    closeAllTabsText: 'Close All Tabs',

    /**
     * @cfg {Array} extraItemsHead
     * An array of additional context menu items to add to the front of the context menu.
     */
    extraItemsHead: null,

    /**
     * @cfg {Array} extraItemsTail
     * An array of additional context menu items to add to the end of the context menu.
     */
    extraItemsTail: null,

    //public
    constructor: function (config) {
        this.addEvents(
            'aftermenu',
            'beforemenu');

        this.mixins.observable.constructor.call(this, config);
    },

    init : function(tabpanel){
        this.tabPanel = tabpanel;
        this.tabBar = tabpanel.down("tabbar");

        this.mon(this.tabPanel, {
            scope: this,
            afterlayout: this.onAfterLayout,
            single: true
        });
    },

    onAfterLayout: function() {
        this.mon(this.tabBar.el, {
            scope: this,
            contextmenu: this.onContextMenu,
            delegate: '.x-tab'
        });
    },

    onBeforeDestroy : function(){
        Ext.destroy(this.menu);
        this.callParent(arguments);
    },

    // private
    onContextMenu : function(event, target){
        var me = this,
            menu = me.createMenu(),
            disableAll = true,
            disableOthers = true,
            tab = me.tabBar.getChildByElement(target),
            index = me.tabBar.items.indexOf(tab);

        me.item = me.tabPanel.getComponent(index);
        menu.child('#close').setDisabled(!me.item.closable);

        if (me.showCloseAll || me.showCloseOthers) {
            me.tabPanel.items.each(function(item) {
                if (item.closable) {
                    disableAll = false;
                    if (item != me.item) {
                        disableOthers = false;
                        return false;
                    }
                }
                return true;
            });

            if (me.showCloseAll) {
                menu.child('#closeAll').setDisabled(disableAll);
            }

            if (me.showCloseOthers) {
                menu.child('#closeOthers').setDisabled(disableOthers);
            }
        }

        event.preventDefault();
        me.fireEvent('beforemenu', menu, me.item, me);

        menu.showAt(event.getXY());
    },

    createMenu : function() {
        var me = this;

        if (!me.menu) {
            var items = [{
                itemId: 'close',
                text: me.closeTabText,
                scope: me,
                handler: me.onClose
            }];

            if (me.showCloseAll || me.showCloseOthers) {
                items.push('-');
            }

            if (me.showCloseOthers) {
                items.push({
                    itemId: 'closeOthers',
                    text: me.closeOthersTabsText,
                    scope: me,
                    handler: me.onCloseOthers
                });
            }

            if (me.showCloseAll) {
                items.push({
                    itemId: 'closeAll',
                    text: me.closeAllTabsText,
                    scope: me,
                    handler: me.onCloseAll
                });
            }

            if (me.extraItemsHead) {
                items = me.extraItemsHead.concat(items);
            }

            if (me.extraItemsTail) {
                items = items.concat(me.extraItemsTail);
            }

            me.menu = Ext.create('Ext.menu.Menu', {
                items: items,
                listeners: {
                    hide: me.onHideMenu,
                    scope: me
                }
            });
        }

        return me.menu;
    },

    onHideMenu: function () {
        var me = this;
        me.fireEvent('aftermenu', me.menu, me);
    },

    onClose : function(){
        this.tabPanel.remove(this.item);
    },

    onCloseOthers : function(){
        this.doClose(true);
    },

    onCloseAll : function(){
        this.doClose(false);
    },

    doClose : function(excludeActive){
        var items = [];

        this.tabPanel.items.each(function(item){
            if(item.closable){
                if(!excludeActive || item != this.item){
                    items.push(item);
                }
            }
        }, this);

        Ext.suspendLayouts();
        Ext.Array.forEach(items, function(item){
            this.tabPanel.remove(item);
        }, this);
        Ext.resumeLayouts(true);
    }
});/**
 * @deprecated
 * Ext.ux.RowExpander has been promoted to the core framework. Use
 * {@link Ext.grid.plugin.RowExpander} instead.  Ext.ux.RowExpander is now just an empty
 * stub that extends Ext.grid.plugin.RowExpander for backward compatibility reasons.
 */
Ext.define('Ext.ux.RowExpander', {
    extend: 'Ext.grid.plugin.RowExpander'
});