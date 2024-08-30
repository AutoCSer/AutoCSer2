/// <reference path = "./load.page.ts" />
'use strict';
AutoCSer.Pub.SendError = function (Error) {
    var Key = Error.LineNo + Error.Message;
    if (Key && !AutoCSer.Pub.Errors[Key]) {
        Error.AppName = navigator.appName;
        Error.AppVersion = navigator.appVersion;
        Error.Location = document.location.toString();
        AutoCSer.Pub.Errors[Key] = Error;
        AutoCSer.HttpRequest.Post('__ERRORPATH__', { message: Error });
    }
};
AutoCSer.HttpRequest.SendError = function (Query) {
    var ErrorPath = '__ERRORPATH__';
    if (Query.Url.substring(0, ErrorPath.length) !== ErrorPath) {
        AutoCSer.Pub.SendError({ Message: '服务器请求失败 : ' + Query.Url + (Query.SendString && !Query.FormData ? ('\r\n' + Query.SendString.length + '\r\n' + Query.SendString.substring(0, 256)) : '') });
    }
};
window.onerror = function (message, filename, lineno, colno, error) {
    if (lineno == 1 && !AutoCSer.Pub.IE) {
        var Ajax = AutoCSer.Pub.AjaxAppendJs;
        if (Ajax.RetryCount && document.location.origin + Ajax.Url == filename && --Ajax.RetryCount) {
            AutoCSer.Pub.AppendJs(Ajax.Url, AutoCSer.Pub.Charset, null, Ajax.GetOnError(null));
            return;
        }
    }
    var SendError = { Message: message }, Location = document.location.toString();
    if (filename && Location.substring(0, filename.length) != filename)
        SendError.FileName = filename;
    if (lineno)
        SendError.LineNo = lineno;
    if (colno)
        SendError.ColNo = colno;
    AutoCSer.Pub.SendError(SendError);
    //if (Pub.IE) {
    //    if ((message != '语法错误' && message != 'Syntax error' && message != '語法錯誤') || Location.substring(0, filename.length) != filename) {
    //        Pub.SendError(SendError);
    //    }
    //}
    //else {
    //    if ((lineno || message != 'Script error.') && (lineno != 1 || message != 'Error loading script')) {
    //        if (lineno == 1) {
    //            var Ajax = Pub.AjaxAppendJs;
    //            if (Ajax.RetryCount && document.location.origin + Ajax.Url == filename && --Ajax.RetryCount) {
    //                Pub.AppendJs(Ajax.Url, Loader.Charset, null, Ajax.GetOnError(null));
    //                return;
    //            }
    //        }
    //        Pub.SendError(SendError);
    //    }
    //}
};
//# sourceMappingURL=loadSendError.js.map