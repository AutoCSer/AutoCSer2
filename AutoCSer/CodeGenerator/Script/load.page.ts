'use strict';
interface Array<T> {
    Copy(): Array<T>;
    RemoveValue(Value: T): Array<T>;
    Remove(IsValue: (Value: T) => boolean): Array<T>;
    RemoveAt(Index: number): Array<T>;
    RemoveAt(Index: number, Count: number): Array<T>;
    RemoveAtEnd(Index: number): Array<T>;
    IndexOf(Function: (T) => boolean): number;
    IndexOfValue(Value: T): number;
    First(Function: (T) => boolean): T;
    MaxValue(): T;
    Max(GetKey: (Value: T) => any): T;
    /**
     * 对象名称解压 ['name0,name1',['value0.0','value0.1'],['value1.0','value1.1']] => [{name0:'value0.0',name1:'value0.1'},{name0:'value1.0',name1:'value1.1'}]
     */
    FormatAjax(): any[];
    /**
     * 数据视图数组解析
     */
    FormatView(): any[];
    Find(IsValue: (Value: T) => boolean): Array<T>;
    ToArray<R>(GetValue: (Value: T) => R): Array<R>;
    For(Function: (Value: T) => any): Array<T>;
    /**
     * 转换为 Object {}
     * @param Function 获取对象属性名称，不传则直接使用数组元素值作为属性名称
     */
    ToHash<K>(Function: (Value: T) => K): Object;
    Sort(GetKey: (Value: any) => number): T[];
    /**
     * 调用 String.fromCharCode 将数字数组转换为字符串
     */
    MakeString(): string;
}
Array.prototype.Copy = function () {
    for (var Index = 0, Array = []; Index !== this.length; Array.push(this[Index++]));
    return Array;
}
Array.prototype.RemoveValue = function (Value: any): any[] {
    return this.Remove(function (Data) { return Data == Value; });
}
Array.prototype.Remove = function (IsValue: (Value: any) => boolean): any[] {
    for (var Index = -1; ++Index != this.length;) {
        if (IsValue(this[Index])) {
            for (var WriteIndex = Index; ++Index != this.length;) {
                if (!IsValue(this[Index])) this[WriteIndex++] = this[Index];
            }
            this.length = WriteIndex;
            break;
        }
    }
    return this;
}
Array.prototype.RemoveAt = function (Index: number, Count = 1) {
    if (Index >= 0) this.splice(Index, Count);
    return this;
}
Array.prototype.RemoveAtEnd = function (Index: number) {
    if (Index >= 0 && Index < this.length) {
        this[Index] = this[this.length - 1];
        --this.length;
    }
    return this;
}
Array.prototype.IndexOf = function (Function: (any) => boolean): number {
    for (var Index = -1; ++Index !== this.length;) {
        if (Function(this[Index])) return Index;
    }
    return -1;
}
Array.prototype.IndexOfValue = function (Value: any): number {
    if (AutoCSer.Pub.IE) {
        for (var Index = -1; ++Index !== this.length;) {
            if (this[Index] == Value) return Index;
        }
        return -1;
    }
    return this.indexOf(Value);
}
Array.prototype.First = function (Function: (any) => boolean): any {
    var Index = this.IndexOf(Function);
    if (Index >= 0) return this[Index];
}
Array.prototype.MaxValue = function (): any {
    if (this.length) {
        for (var Index = this.length, Value = this[--Index]; Index;) {
            if (this[--Index] > Value) Value = this[Index];
        }
        return Value;
    }
}
Array.prototype.Max = function (GetKey: (Value: any) => any): any {
    if (this.length) {
        for (var Index = this.length, Value = this[--Index], Key = GetKey(Value); Index;) {
            var NextKey = GetKey(this[--Index]);
            if (NextKey > Key) {
                Value = this[Index];
                Key = NextKey;
            }
        }
        return Value;
    }
}
Array.prototype.FormatAjax = function (): any[]{
    if (this.length) {
        for (var Index = 0, Names = (this[0] as string).split(','), Values = []; ++Index !== this.length;) {
            for (var Value = this[Index] as any[], NewValue = {},NameIndex = -1; ++NameIndex !== Names.length; NewValue[Names[NameIndex]] = Value[NameIndex]);
            Values.push(NewValue);
        }
        return Values;
    }
}
Array.prototype.FormatView = function (): any[] {
    return this.length > 1 ? AutoCSer.Pub.FormatViewArray(AutoCSer.Pub.GetViewArrayName(this[0], 0), this, 1) : [];
}
Array.prototype.Find = function (IsValue: (Value: any) => boolean): any[] {
    for (var Values = [], Index = 0; Index !== this.length; ++Index) {
        var Value = this[Index];
        if (IsValue(Value)) Values.push(Value);
    }
    return Values;
}
Array.prototype.ToArray = function (GetValue: (Value: any) => any): any[] {
    for (var Values = [], Index = 0; Index !== this.length; Values.push(GetValue(this[Index++])));
    return Values;
}
Array.prototype.For = function (Function: (Value: any) => any): any[] {
    for (var Index = 0; Index !== this.length; Function(this[Index++]));
    return this;
}
Array.prototype.ToHash = function (Function: (Value: any) => any): Object {
    for (var Values = {}, Index = 0; Index !== this.length;) {
        var Value = this[Index++];
        Values[Function ? Function(Value) : Value] = Value;
    }
    return Values;
}
Array.prototype.Sort = function (GetKey: (Value: any) => number): any[] {
    return this.sort(function (Left, Right) { return GetKey(Left) - GetKey(Right); });
}
Array.prototype.MakeString = function (): string {
    return String.fromCharCode.apply(null, this);
}
interface String {
    /**
     * 转移替换字符 <& "'>
     */
    ToHTML(): string;
    /**
     * 转移替换字符 <&>
     */
    ToTextArea(): string;
    Trim(): string;
    PadLeft(Count: number, Char: string): string;
    /**
     * 截取左侧字符串，编码大于 0xff 的算两个字符
     */
    Left(Length: number): string;
    Right(Length: number): string;
    /**
     * 第一个字符转大写字母
     */
    ToLower(): string;
    /**
     * 编码大于 0xff 的算两个字符
     */
    Length(): number;
    /**
     * 分割字符串后调用 parseInt 转成数字数组
     */
    SplitInt(Split: string): number[];
    /**
     * 年月日时分秒 使用一个分隔符 [ :/-] 的字符串解析，比如 2024-4-24-0-3-16
     */
    ParseDate(): Date;
}
String.prototype.ToHTML = function (): string {
    return this.ToTextArea().replace(/ /g, '&nbsp;').replace(/"/g, '&#34;').replace(/'/g, '&#39;');
    //.replace(/\\/g, '&#92;');
};
String.prototype.ToTextArea = function (): string{
    return this.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
};
String.prototype.Trim = function (): string {
    return this.replace(/(^\s*)|(\s*$)/g, '');
}
String.prototype.PadLeft = function (Count: number, Char: string): string {
    var Value = '';
    if ((Count -= this.length) > 0) {
        for (Char = Char.charAt(0); Count; Char += Char, Count >>= 1) {
            if (Count & 1) Value += Char;
        }
    }
    return Value + this;
}
String.prototype.Left = function (Length: number): string {
    for (var Value = 0, Index = -1; ++Index != this.length && Length > 0;)	if ((Length -= this.charCodeAt(Index) > 0xff ? 2 : 1) >= 0)++Value;
    return this.substring(0, Value);
}
String.prototype.Right = function (Length: number): string {
    return this.length > Length ? this.substring(this.Length - Length) : this;
}
String.prototype.ToLower = function ():string {
    return this.substring(0, 1).toLowerCase() + this.substring(1);
}
String.prototype.Length = function ():number {
    for (var Value = this.length, Index = 0; Index - this.length;)	if (this.charCodeAt(Index++) > 0xff)++Value;
    return Value;
}
String.prototype.SplitInt = function (Split: string): number[] {
    var Value = this.Trim();
    return Value.length ? this.split(Split).ToArray(function (Value) { return parseInt(0 + Value); }) : [];
}
String.prototype.ParseDate = function (): Date {
    var Value = this.Trim();
    if (Value) {
        var DateValue = new Date(Value = Value.replace(/\-/g, '/'));
        if (!isNaN(DateValue.getTime())) return DateValue;
        Value = Value.replace(/[ :\/]+/g, ' ').split(' ');
        DateValue = new Date(Value[0], parseInt(Value[1]) - 1, Value[2], Value[3], Value[4], Value[5]);
        if (!isNaN(DateValue.getTime())) return DateValue;
    }
}
interface Date {
    AddMilliseconds(Value: number): Date;
    AddMinutes(Value: number): Date;
    AddHours(Value: number): Date;
    AddDays(Value: number): Date;
    /**
     * 时间值转字符串
     * @param Format 包含 yMdhmsSw 的自定义格式化字符串，y 表示年，M 表示月，d 表示日，h 表示时，m 表示分，s 表示秒，S 表示毫秒，w 为汉字 日/一/二/三/四/五/六
     * @param IsFixed 是否根据 Format 的重复字符数量固定输出字符串长度，比如 yyyyy 输出 02024
     */
    ToString(Format: string, IsFixed: boolean): string;
    /**
     * 时间值转日期字符串 yyyy/MM/dd
     */
    ToDateString(): string;
    /**
     * 时间值转字符串 HH:mm:ss
     */
    ToTimeString(): string;
    /**
     * 时间值转字符串 yyyy/MM/dd HH:mm
     */
    ToMinuteString(): string;
    /**
     * 时间值转字符串 yyyy/MM/dd HH:mm:ss
     */
    ToSecondString(): string;
    /**
     * 时间值为今天则转换为 HH:mm 否则转换为 yyyy/MM/dd
     */
    ToMinuteOrDateString(): string;
    /**
     * 日期转二进制数字 (y << 9) + ((M + 1) << 5) + d
     */
    ToInt(): number;
}
Date.prototype.AddMilliseconds = function (Value: number): Date {
    var NewDate = new Date;
    NewDate.setTime(this.getTime() + Value);
    return NewDate;
}
Date.prototype.AddMinutes = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60);
}
Date.prototype.AddHours = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60 * 60);
}
Date.prototype.AddDays = function (Value: number) {
    return this.AddMilliseconds(Value * 1000 * 60 * 60 * 24);
}
Date.prototype.ToString = function (Format: string, IsFixed = true): string {
    var Value: { [key: string]: number } = { y: this.getFullYear(), M: this.getMonth() + 1, d: this.getDate(), h: this.getHours(), m: this.getMinutes(), s: this.getSeconds(), S: this.getMilliseconds() };
    Format = Format.replace(/Y/g, 'y').replace(/D/g, 'd').replace(/H/g, 'h').replace(/W/g, 'w');
    if (IsFixed) {
        Format += Value[Format.charAt(Format.length - 1)] ? '.' : 'y';
        for (var Values = [], LastChar = '', LastIndex = 0, Index = -1; ++Index - Format.length; LastChar = Char) {
            var Char = Format.charAt(Index);
            if (Char != LastChar) {
                if (Value[LastChar] != null) {
                    Values.push(Value[LastChar].toString().Right(Index - LastIndex).PadLeft(Index - LastIndex, '0'));
                    LastIndex = Index;
                }
                else if (Value[Char] != null) Values.push(Format.substring(LastIndex, LastIndex = Index));
            }
        }
        Format = Values.join('');
    }
    else {
        for (var Name in Value) Format = Format.replace(new RegExp(Name, 'g'), Value[Name].toString());
    }
    return Format.replace(/w/g, ['日', '一', '二', '三', '四', '五', '六'][this.getDay()]);
};
Date.prototype.ToDateString = function (): string { return this.ToString('yyyy/MM/dd'); };
Date.prototype.ToTimeString = function (): string { return this.ToString('HH:mm:ss'); };
Date.prototype.ToMinuteString = function (): string { return this.ToString('yyyy/MM/dd HH:mm'); };
Date.prototype.ToSecondString = function (): string { return this.ToString('yyyy/MM/dd HH:mm:ss'); };
Date.prototype.ToMinuteOrDateString = function () { return this.ToInt() == new Date().ToInt() ? this.ToString('HH:mm') : this.ToDateString(); };
Date.prototype.ToInt = function (): number {
    return (this.getFullYear() << 9) + ((this.getMonth() + 1) << 5) + this.getDate();
}
interface Number {
    /**
     * 二进制数字转日期 (y << 9) + ((M + 1) << 5) + d
     */
    IntToDate(): Date;
    /**
     * 数据视图转 style.display 值，0 表示不显示 none，否则为空字符串
     */
    ToDisplay(): string;
    /**
     * 数据视图转 style.display 值，0 表示空字符串，否则为不显示 none
     */
    ToDisplayNone(): string;
    /**
     * 数据视图转逻辑值，比如用于表示 checked 值，0 表示 false，否则为 true
     */
    ToTrue(): boolean;
    /**
     * 数据视图转逻辑值，比如用于表示 checked 值，0 表示 true，否则为 false
     */
    ToFalse(): boolean;
}
Number.prototype.IntToDate = function (): Date { return new Date(this >> 9, ((this >> 5) & 15) - 1, this & 31); };
Number.prototype.ToDisplay = function (): string { return this.toString() == '0' ? 'none' : ''; };
Number.prototype.ToDisplayNone = function (): string { return this.toString() == '0' ? '' : 'none'; };
Number.prototype.ToTrue = function (): boolean{ return this.toString() != '0'; };
Number.prototype.ToFalse = function (): boolean { return this.toString() == '0'; };
interface Boolean {
    /**
     * 数据视图转 style.display 值，true 表示空字符串，否则为不显示 none
     */
    ToDisplay(): string;
    /**
     * 数据视图转 style.display 值，true 表示不显示 none，否则为空字符串
     */
    ToDisplayNone(): string;
    /**
     * 数据视图转逻辑值，比如用于表示 checked 值，true 表示 true，否则为 false
     */
    ToTrue(): boolean;
    /**
     * 数据视图转逻辑值，比如用于表示 checked 值，true 表示 false，否则为 true
     */
    ToFalse(): boolean;
}
Boolean.prototype.ToDisplay = function () { return this.toString() == 'true' ? '' : 'none'; };
Boolean.prototype.ToDisplayNone = function () { return this.toString() == 'true' ? 'none' : ''; };
Boolean.prototype.ToTrue = function () { return this.toString() == 'true'; };
Boolean.prototype.ToFalse = function () { return this.toString() != 'true'; };
module AutoCSer {
    interface IArray {
        [index: number]: any;
        length: number;
    }
    interface IModuleLoader {
        OnLoad: Function;
        Count: number;
        IsLoad: boolean;
        Paths: {};
    }
    interface IViewArrayName {
        Names: IViewArrayName[];
        Name: string;
        Index: number;
        ViewType: string;
    }
    export interface ISendError {
        AppName: string;
        AppVersion: string;
        Location: string;
        FileName: string;
        LineNo: number;
        ColNo: number;
        Message: string;
        Caller: string,
    }
    interface LoadParameter {
        LoadScript: boolean;
        ViewPath: string;
    }
    export class Pub {
        /**
        * 调用 window.alert
        */
        static Alert;
        /**
        * 模拟类型继承操作
        */
        static Extends(Son: Function, Base: Function) {
            for (var Name in Base) if (Base.hasOwnProperty(Name)) Son[Name] = Base[Name];
            function Constructor() { this.constructor = Son; }
            Son.prototype = Base === null ? Object.create(Base) : (Constructor.prototype = Base.prototype, new Constructor());
        }
        /**
        * 具有 length 属性的数据转换为数组
        * @param Value 具有 length 属性的数据
        */
        static ToArray(Value: IArray, StartIndex = 0): any[] {
            for (var Values = []; StartIndex < Value.length; ++StartIndex) Values.push(Value[StartIndex]);
            return Values;
        }
        /**
        * SendError 调用的错误信息缓存，避免发送重复数据
        */
        static Errors: { [key: string]: ISendError; } = {}
        /**
        * 将错误信息发送给服务器，用于采集客户端错误信息日志，采用 GET 请求所以信息长度不能过长
        * @param Error 属性 LineNo+Message 为缓存关键字
        */
        static SendError(Error: ISendError) { }
        /**
        * ThisFunction / ThisEvent 调用是否将使用 try catch 拦截异常信息
        */
        static IsTryError = true;
        /**
        * 绑定函数的 this 值
        * @param This 函数 Function 绑定的 this 对象
        * @param Function 被绑定 this 对象的函数
        * @param Arguments 函数 Function 调用的附加参数，添加到原始传参的后面
        * @param IsArgument 是否传参原始参数，否则抛弃原始传参仅传参附加参数
        */
        static ThisFunction<T extends Function>(This: any, Function: T, Arguments: any[] = null, IsArgument = true): T {
            if (Function) {
                var Value = function () {
                    if (Pub.IsTryError) {
                        try {
                            return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
                        } catch (e) {
                            Pub.SendError({ Message: (e ? (e.stack || e).toString() : '未知错误') + '\r\n' + Function.toString() } as ISendError);
                        }
                    }
                    return Function.apply(This, IsArgument ? Pub.ToArray(arguments).concat(Arguments || []) : Arguments);
                } as Object as T;
                return Value;
            }
            var SendError = { Message: 'Function is null' } as ISendError, Caller = window['caller'];
            if (Caller) SendError.Caller = Caller;
            Pub.SendError(SendError);
        }
        /**
        * 绑定事件函数的 this 值，返回 BrowserEvent.Return
        * @param This 事件函数 Function 绑定的 this 对象
        * @param Function 被绑定 this 对象的事件函数
        * @param Arguments 函数 Function 调用的附加参数，添加到 BrowserEvent 参数的后面
        * @param Frame 事件参数 event 的来源框架
        */
        static ThisEvent(This: any, Function: Function, Arguments: any[] = null, Frame: Window = null): (Event: Event) => boolean {
            if (Function) {
                return function (Event: Event) {
                    var Browser = new BrowserEvent(Pub.IE ? Frame ? Frame.event || event : event : Event);
                    if (Pub.IsTryError) {
                        try {
                            Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
                        } catch (e) {
                            Pub.SendError({ Message: (e ? (e.stack || e).toString() : '未知错误') + '\r\n' + Function.toString() } as ISendError);
                        }
                    }
                    else Function.apply(This, Arguments ? [Browser].concat(Arguments || []) : [Browser]);
                    return Browser.Return;
                };
            }
            var SendError = { Message: 'Event is null' } as ISendError, Caller = window['caller'];
            if (Caller) SendError.Caller = Caller;
            Pub.SendError(SendError);
        }
        /**
         * 转换为 script 的当前 GET 请求对象
         */
        static AjaxAppendJs: HttpRequestQueryInfo;
        /**
         * 全局 AJAX 回调函数
         */
        static AjaxCallBack: Function;
        /**
         * 对象属性复制
         */
        static Copy(Left: Object, Right: Object): Object {
            for (var Name in Right) Left[Name] = Right[Name];
            return Left;
        }
        /**
         * 数据视图数组对象名称解析
         */
        static GetViewArrayName(Name: string, StartIndex: number): IViewArrayName {
            for (var Values = { Names: [] } as IViewArrayName, Index = StartIndex; Index - Name.length;) {
                var Code = Name.charCodeAt(Index);
                //[
                if (Code === 91) {
                    var Value = this.GetViewArrayName(Name, Index + 1);
                    Value.Name = Name.substring(StartIndex, Index);
                    StartIndex = Index = Value.Index;
                    Values.Names.push(Value);
                }
                //]
                else if (Code === 93) {
                    var SubName = Name.substring(StartIndex, Index++);
                    if (SubName) Values.Names.push({ Name: SubName } as IViewArrayName);
                    Values.Index = Index;
                    return Values;
                }
                //,
                else if (Code === 44) {
                    var SubName = Name.substring(StartIndex, Index++);
                    if (SubName) {
                        //@
                        if (SubName.charCodeAt(0) === 64) Values.ViewType = SubName.substring(1);
                        else Values.Names.push({ Name: SubName } as IViewArrayName);
                    }
                    StartIndex = Index;
                }
                else ++Index;
            }
            if (StartIndex - Name.length) {
                var SubName = Name.substring(StartIndex);
                //@
                if (SubName.charCodeAt(0) === 64) Values.ViewType = SubName.substring(1);
                else Values.Names.push({ Name: SubName } as IViewArrayName);
            }
            return Values;
        }
        /**
         * 数据视图数组对象解析
         */
        static FormatViewArray(Name: IViewArrayName, Values: any[], StartIndex: number): any[] {
            for (var Value = [], Index = StartIndex - 1; ++Index - Values.length; Value.push(Values[Index] ? this.FormatViewArrayItem(Name, Values[Index]) : null));
            return Value;
        }
        /**
         * 数据视图数组对象元素值解析
         */
        static FormatViewArrayItem(Name: IViewArrayName, Values: any[]): any {
            var Names = Name.Names;
            if (Names && Names.length) {
                if (Names[0].Name) {
                    for (var Value = {}, Index = Names.length; --Index >= 0;) {
                        Value[Names[Index].Name] = Values[Index] != null ? (Names[Index].Names && Names[Index].Names.length ? this.FormatViewArrayItem(Names[Index], Values[Index] as any[]) : Values[Index]) : null;
                    }
                    if (Name.ViewType) {
                        if (Name.ViewType.charAt(0) == '.') Value = eval(Name.ViewType.substring(1) + '.Get(Value)');
                        else Value = eval('new ' + Name.ViewType + '(Value)');
                    }
                    return Value;
                }
                else if (Values[0]) return this.FormatViewArray(Names[0], Values[0], 0);
            }
            else return Values;
        }
        /**
         * 创建 script 组件
         */
        static CreateJavaScript(Src: string, Charset = Pub.Charset): HTMLScriptElement {
            var Script = document.createElement('script');
            Script.lang = 'javascript';
            Script.type = 'text/javascript';
            Script.src = Src;
            Script.charset = Charset;
            return Script;
        }
        /**
         * 添加 script 组件
         */
        static AppendJs(Src: string, Charset = this.Charset, OnLoad: Function = null, OnError: Function = null): void {
            if (OnLoad || OnError) new LoadJs(this.CreateJavaScript(Src, Charset), OnLoad, OnError);
            else this.DocumentHead.appendChild(this.CreateJavaScript(Src, Charset));
        }
        /**
         * 对象转 JSON 字符串
         * @param IsIgnore 对象属性值是否忽略转逻辑非 true 值
         * @param IsNameQuery 默认为 true 表示属性名称转 JSON 字符串，如果确定属性名称为字母则可以传参 false 减少开销
         * @param IsSortName 默认为 true 表示输出属性按照名称排序
         * @param Parents 历史对象，用于检查循环引用
         */
        static ToJson(Value, IsIgnore = false, IsNameQuery = true, IsSortName = true, Parents: any[] = null) {
            if (Value != null) {
                var Type = typeof (Value);
                if (Type != 'function') {
                    if (Type == 'string') return '"' + Value.toString().replace(/[\\"]/g, '\\$&').replace(/\n/g, '\\n') + '"';
                    if (Type == 'number' || Type == 'boolean') return Value.toString();
                    var ApplyType = Object.prototype.toString.apply(Value);
                    if (ApplyType == '[object Date]') return 'new Date(' + Value.getTime() + ')';
                    if (!Parents) Parents = [];
                    for (var Index = 0; Index - Parents.length; ++Index) if (Parents[Index] == Value) return 'null';
                    if (typeof (Value.ToJson) == 'function') return Value.ToJson(IsIgnore, IsNameQuery, IsSortName, Parents);
                    Parents.push(Value);
                    var Values = [] as string[];
                    if (ApplyType == '[object Array]') {
                        for (var Index = 0; Index - Value.length; ++Index)    Values.push(this.ToJson(Value[Index], IsIgnore, IsNameQuery, IsSortName, Parents));
                        Parents.pop();
                        return '[' + Values.join(',') + ']';
                    }
                    for (var Name in Value) {
                        var NextValue = Value[Name];
                        if (NextValue !== undefined) {
                            if ((!IsIgnore || NextValue) && typeof (NextValue) != 'function') Values.push(Name);
                        }
                    }
                    if (Values.length) {
                        if (IsSortName) Values.sort();
                        for (var Index = Values.length; Index;) {
                            var Name = Values[--Index];
                            Values[Index] = (IsNameQuery ? this.ToJson(Name.toString()) : Name.toString()) + ':' + this.ToJson(Value[Name], IsIgnore, IsNameQuery, true, Parents);
                        }
                    }
                    Parents.pop();
                    return '{' + Values.join(',') + '}';
                }
            }
            return 'null';
        }
        /**
         * 对象转 URL 查询字符串
         * @param IsIgnore 对象属性值是否忽略转逻辑非 true 值
         */
        static ToQuery(Value, IsIgnore = false): string {
            var Values = [];
            for (var Name in Value) {
                if (Value[Name] != null) {
                    var Type = typeof (Value[Name]);
                    if (Type != 'function' && (!IsIgnore || Value[Name])) Values.push(encodeURI(Name) + '=' + encodeURI(Value[Name].toString()));
                }
            }
            return Values.join('&');
        }
        /**
         * 当前 URL 查询对象（包含 # 后面的查询）
         */
        static Query: { [key: string]: any; };
        /**
         * 获取 URL 查询字符串（? 后面的查询字符串）
         */
        static GetLocationSearch(Location: Location = location): string {
            return Location.search.toString().replace(/^\?/g, '');
        }
        /**
         * 获取 URL 哈希字符串（# 或者 #! 后面的字符串）
         */
        static GetLocationHash(Location: Location = location): string {
            return Location.hash.toString().replace(/^#(\!|\%21)?/g, '');
        }
        /**
         * 查询字符串解析到对象属性值
         */
        static FillQuery(Values: { [key: string]: any; }, Search: string, IsStaticVersion: boolean) {
            var Query = Search.split('&');
            if (Query.length == 1 && Search.indexOf('=') == -1) Values[''] = decodeURI(Search);
            else {
                for (var Index = Query.length; --Index >= 0;) {
                    var KeyValue = Query.pop().split('='), Key = decodeURI(KeyValue[0]), KeyType = -1;
                    if (Key.indexOf(':') == Key.length - 2) {
                        switch (KeyType = Key.charCodeAt(Key.length - 1) - 0x66) {
                            //f
                            case 0x66 - 0x66:
                            //i
                            case 0x69 - 0x66:
                            //j
                            case 0x6A - 0x66:
                                Key = Key.substring(0, Key.length - 2);
                                break;
                        }
                    }
                    if (IsStaticVersion || Key != '__VERSIONQUERYNAME__') {
                        var Value = KeyValue.length < 2 ? '' : decodeURI(KeyValue[1]);
                        try {
                            switch (KeyType) {
                                //f
                                case 0x66 - 0x66: Values[Key] = parseFloat(Value); break;
                                //i
                                case 0x69 - 0x66: Values[Key] = parseInt(Value); break;
                                //j
                                case 0x6A - 0x66: eval('Values[Key]=' + Value); break;
                                default: Values[Key] = Value; break;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        /**
         * 创建查询对象
         */
        static CreateQuery(Location: Location): { [key: string]: string; } {
            var Value: { [key: string]: string; } = {}, Search = this.GetLocationSearch(Location), Hash = this.GetLocationHash(Location);
            if (Hash.length) this.FillQuery(Value, Hash, this.IsStaticVersion);
            if (Search.length) this.FillQuery(Value, Search, false);
            return Value;
        }
        /**
         * 是否已经初始化加载完毕
         */
        private static IsLoad: boolean;
        /**
         * 初始化加载完毕以后需要执行的函数
         */
        private static OnLoads: Function[] = [];
        /**
         * # 查询变化加载完毕事件
         */
        static OnLoadedHash: Events;
        /**
         * 添加初始化加载完毕以后需要执行的函数
         * @param This 函数 OnLoad 绑定的 this 对象
         * @param IsOnce 默认为 false 表示触发 OnLoadedHash 事件执行
         */
        static OnLoad(OnLoad: Function, This: Object = null, IsOnce = false) {
            if (This) OnLoad = this.ThisFunction(This, OnLoad);
            if (!IsOnce) this.OnLoadedHash.Add(OnLoad);
            if (this.IsLoad) OnLoad();
            else this.OnLoads.push(OnLoad);
        }
        /**
         * Path 为关键字的模块加载状态，false 表示正在加载中，true 表示已经加载完成
         */
        static IsModules: { [key: string]: boolean } = {};
        /**
         * Path 为关键字的模块加载完成事件
         */
        static LoadModules: { [key: string]: IModuleLoader[] } = {};
        /**
         * 模块脚本加载完以后调用该函数注册模块信息
         * @param Path 相对 __JAVASCRIPTPATH__ 的脚本文件地址，不包括文件扩展名 .js，比如 Path='htmlEditor'
         */
        static LoadModule(Path: string) {
            if (this.IsModules[Path] == null) Pub.SendError({ Message: '加载了未知模块 ' + Path } as ISendError);
            else {
                this.IsModules[Path] = true;
                for (var Loads = this.LoadModules[Path], Index = Loads ? Loads.length : 0; --Index >= 0;) {
                    var Load = Loads[Index];
                    if (Load && Load.Paths[Path]) {
                        Load.Paths[Path] = 0;
                        if (--Load.Count == 0 && Load.OnLoad) {
                            if (Load.IsLoad) this.OnLoad(Load.OnLoad);
                            else Load.OnLoad();
                        }
                    }
                }
            }
        }
        /**
         * 注册模块加载完以后需要触发的调用函数
         * @param Paths 需要加载的模块路径集合
         * @param OnLoad 所有模块加载完成以后需要触发的调用函数
         * @param IsLoad 默认为 true 表示需要等待 AutoCSer 初始化加载完成以后才触发函数调用
         * @param Version 默认与数据视图定义版本号一致
         */
        static OnModule(Paths: string[], OnLoad: Function, IsLoad: boolean = true, Version: string = this.Version) {
            for (var Index = Paths.length, Load = { IsLoad: IsLoad, OnLoad: OnLoad, Count: 0, Paths: {} } as IModuleLoader; Index;) {
                var Path = Paths[--Index];
                if (!this.IsModules[Path]) {
                    ++Load.Count;
                    var Loads = this.LoadModules[Path];
                    if (!Loads) this.LoadModules[Path] = Loads = [];
                    Load.Paths[Path] = 1;
                    Loads.push(Load);
                    this.LoadModuleWhenNull(Path, Version);
                }
            }
            if (!Load.Count && OnLoad) {
                if (IsLoad) this.OnLoad(OnLoad);
                else OnLoad();
            }
        }
        /**
         * 触发模块加载
         */
        private static LoadModuleWhenNull(Path: string, Version: string) {
            if (this.IsModules[Path] == null) {
                this.IsModules[Path] = false;
                this.AppendJs(this.JsDomain + '__JAVASCRIPTPATH__/' + Path + '.js?__VERSIONQUERYNAME__=' + Version);
            }
        }
        /**
         * 数据视图全局客户端对象
         */
        static ClientViewData: any = {};
        /**
         * 加载数据视图
         * @param IsReView 是否 # 变化以后重新加载的数据视图
         */
        static LoadView(View: PageView, IsReView: boolean) {
            //if (View.Location) location.replace(View.Location);
            if ((View as Object as IHttpRequestReturn).ErrorRequest || View.State != 1 || !View.Result) {
                this.PageView.IsLoadView = this.PageView.IsLoad = this.PageView.IsView = this.PageView.LoadError = true;
                this.ReadyState();
                return;
            }
            (View.Result.Client = this.ClientViewData).Query = this.Query;
            if (!IsReView) {
                //View.OnShowed = this.PageView.OnShowed;
                //View.OnSet = this.PageView.OnSet;
                View.IsLoadView = View.IsLoad = View.IsView = true;
                this.PageView = View;
                this.ReadyState();
            }
        }
        /**
         * 尝试重新加载数据视图
         */
        static ReLoad() {
            var ViewOver = document.getElementById('__VIEWOVERID__');
            if (ViewOver) ViewOver.innerHTML = '正在尝试重新加载视图数据...';
            var Query = this.CreateLoadViewQuery();
            if (!Query.IsStaticVersion) Query.IsRandom = true;
            HttpRequest.GetQuery(Query);
        }
        /**
         * 默认为true 表示 # 字符串更新重新加载数据视图以后将滚动条移动到顶部
         */
        static LoadHashScrollTop = true;
        /**
         * # 字符串被修改以后重新加载数据视图
         */
        static LoadHash(PageView: PageView) {
            if (!(PageView as Object as IHttpRequestReturn).ErrorRequest && PageView.State == 1 && PageView.Result) {
                this.OnBeforeUnLoad.Function();
                var Data = View.Body.GetData().$Data, Result = PageView.Result;
                for (var Name in Result) Data[Name] = Result[Name];
                this.OnLoadHash.Function(PageView.Result = Data);
                this.LoadView(PageView, true);
                View.Body.Show(Data);
                View.ChangeHeader();
                this.OnLoadedHash.Function();
                if (this.LoadHashScrollTop) HtmlElement.$SetScrollTop(0);
            }
        }
        /**
         * # 字符串更新触发重新请求以后的操作事件处理
         */
        static OnQueryEvents: Events;
        /**
         * 定时检查 # 字符串是否被修改
         */
        private static CheckHashFunction: Function;
        /**
         * 检查 # 字符串是否被修改 onhashchange
         */
        static CheckHash(Event: HashChangeEvent): any {
            var Hash = Pub.GetLocationHash();
            if (Hash !== this.LocationHash) {
                this.LocationHash = Hash;
                this.Query = Pub.CreateQuery(location);
                this.ReView();
                Pub.OnQueryEvents.Function();
            }
            if (Pub.IE) setTimeout(this.CheckHashFunction, 100);
        }
        /**
         * # 字符串被修改以后重新加载数据视图请求
         */
        static ReView() {
            var Query = new HttpRequestQuery(this.ViewPath, this.Query, this.ThisFunction(this, this.LoadHash), this.IsStaticVersion);
            if (!Query.IsStaticVersion) Query.IsRandom = true;
            HttpRequest.GetQuery(Query);
        }
        /**
         * 页面初始化加载是否完成
         */
        private static LoadComplete: boolean;
        /**
         * 检查页面初始化加载状态函数
         */
        private static ReadyFunction: Function;
        /**
         * 检查页面初始化加载状态
         */
        static ReadyState() {
            var View = this.PageView, IsLoad = document.body && (document.readyState == null || document.readyState.toLowerCase() === 'complete');
            if (IsLoad && !this.LoadComplete) {
                HtmlElement.$('@body').To();
                AutoCSer.View.Create(View);
                this.DeleteElements = HtmlElement.$Create('div').Styles('padding,margin', '10px').Style('border', '10px solid red').Opacity(0).To();
                this.IsBorder = this.DeleteElements.XY0().Left - 10;
                this.DeleteElements.Styles('padding,margin,border', '0px');
                this.IsBorder -= this.DeleteElements.XY0().Left;
                if (this.IsBorder === 20) this.IsPadding = true;
                this.IsFixed = Pub.IE ? this.DeleteElements.Style('position', 'fixed').Style('left', '50%').Element0().offsetLeft : 1;
                this.DeleteElements.Display(0);
                this.LoadComplete = true;
            }
            if (IsLoad && View.IsLoad) {
                if (View.IsLoadView) {
                    if (View.LoadError) {
                        View.IsLoad = View.IsLoadView = View.LoadError = false;
                        var ViewOver = document.getElementById('__VIEWOVERID__');
                        if (ViewOver) ViewOver.innerHTML = '错误：视图数据加载失败，稍后尝试重新加载';
                        document.title = 'Server Error';
                        setTimeout(this.ThisFunction(this, this.ReLoad), 2000);
                        return;
                    }
                    AutoCSer.View.Body.Show(View.Result);
                    AutoCSer.View.ChangeHeader();
                }
                else {
                    document.body.innerHTML = document.body.innerHTML.replace(/ @(src|style)=/gi, ' $1=');
                    var ViewOver = document.getElementById('__VIEWOVERID__');
                    if (ViewOver) document.body.removeChild(ViewOver);
                }
                this.OnReadyState();
                this.IsLoad = true;
                for (var Index = -1; ++Index - this.OnLoads.length; this.OnLoads[Index]());
                this.OnLoads = this.ReadyFunction = null;
            }
            else setTimeout(this.ReadyFunction, 1);
        }
        /**
         * URL 哈希字符串（# 或者 #! 后面的字符串）
         */
        static LocationHash: string;
        /**
         * 删除控件临时父节点
         */
        static DeleteElements: HtmlElement;
        /**
         * focus 事件
         */
        static FocusEvents: Events;
        /**
         * css style 是否支持 border-left-width 与 border-top-width
         */
        static IsBorder: number;
        /**
         * css style 是否支持固定位置 position:fixed
         */
        static IsFixed: number;
        /**
         * css style 是否支持 padding-left 与 padding-top
         */
        static IsPadding: boolean;
        /**
         * 页面初始化完成操作
         */
        static OnReadyState() {
            this.LocationHash = Pub.GetLocationHash();
            HtmlElement.$(document.body).AddEvent('focus', this.FocusEvents = new Events());
            if (Pub.IE) setTimeout(this.CheckHashFunction = this.ThisFunction(this, this.CheckHash), 100);
            else window.onhashchange = this.ThisFunction(this, this.CheckHash);
        }
        /**
         * 创建数据视图请求参数
         */
        static CreateLoadViewQuery(): HttpRequestQuery {
            return new HttpRequestQuery(this.ViewPath, this.Query, this.ThisFunction(this, this.LoadView), this.IsStaticVersion);
        }
        /**
         * 渲染数据视图数据之前的数据处理操作，传参为视图数据
         */
        static OnLoadHash: Events;
        /**
         * onbeforeunload 事件
         */
        static OnBeforeUnLoad: Events;
        /**
         * 全局数据视图对象
         */
        static PageView: PageView;
        /**
         * 是否 Internet Explorer 浏览器 
         */
        static IE: boolean;
        /**
         * AutoCSer 组件全局初始化入口，调用方式应该为 setTimeout(AutoCSer.Pub.LoadIE, 0, 'javascript');
         */
        static LoadIE():void {
            Pub.IE = !arguments.length || navigator.appName == 'Microsoft Internet Explorer';
            HttpRequest.Load();
            Pub.Load();
        }
        /**
         * document.getElementsByTagName('head')[0]
         */
        static DocumentHead: HTMLHeadElement;
        /**
         * JavaScript 文件默认加载域名
         */
        static JsDomain: string;
        /**
         * charset
         */
        static Charset: string;
        /**
         * 文件版本
         */
        static Version: string;
        /**
         * 是否全局静态版本
         */
        static IsStaticVersion: boolean;
        /**
         * 视图数据请求地址
         */
        static ViewPath: string;
        /**
         * 初始化加载数据
         */
        private static Load() {
            this.DocumentHead = document.getElementsByTagName('head')[0];
            for (var Nodes = this.DocumentHead.childNodes, Index = Nodes.length; Index;) {
                var Node = Nodes[--Index] as HTMLScriptElement;
                if (!Node.tagName || !Node.src || Node.tagName.toLowerCase() != 'script') continue;
                var LoadMatch = Node.src.match(/^(https?:\/\/[^\/]+\/)__JAVASCRIPTPATH__\/load\.js\?__VERSIONQUERYNAME__=([\dA-F]+)#(s?)(\/.*)?$/i);
                if (LoadMatch && (this.JsDomain = LoadMatch[1])) {
                    this.Version = LoadMatch[2];
                    this.IsStaticVersion = !!LoadMatch[3];
                    this.ViewPath = LoadMatch[4];
                    this.Charset = Node.charset;
                    break;
                }
            }
            if (!this.JsDomain) this.JsDomain = '/';
            this.OnLoadHash = new Events();
            this.OnLoadedHash = new Events();
            this.OnBeforeUnLoad = new Events();
            window.onbeforeunload = function (Event: BeforeUnloadEvent) { Pub.OnBeforeUnLoad.Function(Event); }
            this.Query = this.CreateQuery(self.location);
            this.PageView = new PageView();
            this.ReadyFunction = this.ThisFunction(this, this.ReadyState);
            if (this.ViewPath) {
                this.PageView.IsLoadView = true;
                HttpRequest.GetQuery(this.CreateLoadViewQuery());
            }
            else {
                this.PageView.IsLoad = true;
                this.ReadyState();
            }
        }
        /**
         * 声明组件构造函数集合，关键字为 组件名称
         */
        static Functions: { [key: string]: Function } = {};
        /**
         * 声明组件初始化复制参数属性
         * @param Value 组件实例对象
         * @param DefaultParameter 默认复制参数对象，定义复制属性名称与默认值，一般为静态单例
         * @param Parameter 指定复制参数对象，一般为构造函数传参
         */
        static GetParameter(Value, DefaultParameter, Parameter = null) {
            if (Parameter) {
                for (var Name in DefaultParameter) {
                    var ParameterValue = Parameter[Name];
                    Value[Name] = ParameterValue == null ? DefaultParameter[Name] : ParameterValue;
                }
            }
            else {
                for (var Name in DefaultParameter) Value[Name] = DefaultParameter[Name];
            }
        }
        /**
         * 声明组件初始化复制自定义事件参数属性
         * @param Value 组件实例对象
         * @param DefaultEvents 默认复制自定义事件参数对象，定义复制自定义事件属性名称，一般为静态单例
         * @param Parameter 指定复制自定义事件参数对象，一般为构造函数传参
         */
        static GetEvents(Value, DefaultEvents, Parameter = null) {
            if (Parameter) {
                for (var Name in DefaultEvents) {
                    var Function = Parameter[Name] as Function, Event = new Events();
                    if (Function) Event.Add(Function);
                    Value[Name] = Event;
                }
            }
            else {
                for (var Name in DefaultEvents) Value[Name] = new Events();
            }
        }
        /**
         * 声明组件初始化复制自定义事件与参数属性
         * @param Value 组件实例对象
         * @param DefaultParameter 默认复制参数对象，定义复制属性名称与默认值，一般为静态单例
         * @param DefaultEvents 默认复制自定义事件参数对象，定义复制自定义事件属性名称，一般为静态单例
         * @param Parameter 指定复制参数对象，一般为构造函数传参
         */
        static GetParameterEvents(Value, DefaultParameter, DefaultEvents, Parameter = null) {
            Pub.GetParameter(Value, DefaultParameter, Parameter);
            Pub.GetEvents(Value, DefaultEvents, Parameter);
        }
        /**
         * 全局唯一标识
         */
        private static Identity = 0;
        /**
         * 获取全局唯一标识
         */
        static GetIdentity() { return ++this.Identity; }
    }
    Pub.Alert = Pub.ThisFunction(window, alert);
    export interface IIndexPool {
        /**
         * 索引编号
         */
        PoolIndex: number;
        /**
         * 标识符号
         */
        PoolIdentity: number;
    }
    class IndexPoolNode {
        /**
         * 标识符号
         */
        Identity: number;
        /**
         * 保存对象
         */
        Value: IIndexPool;
        /**
         * 设置保存对象
         */
        Set(Value: IIndexPool) {
            (this.Value = Value).PoolIdentity = ++this.Identity;
        }
        /**
         * 移除保存对象
         */
        Pop(Value: IIndexPool): boolean {
            if (Value.PoolIdentity === this.Identity) {
                this.Value = null;
                Value.PoolIdentity = 0;
                return true;
            }
            return false;
        }
        /**
         * 获取保存对象
         */
        Get(Identity: number): IIndexPool {
            return Identity === this.Identity ? this.Value : null;
        }
    }
    export class IndexPool {
        /**
         * 保存对象节点集合
         */
        static Nodes: IndexPoolNode[] = [];
        /**
         * 空闲索引集合
         */
        static Indexs: number[] = [];
        /**
         * 添加保存对象
         */
        static Push(Value: IIndexPool) {
            if (this.Indexs.length) this.Nodes[Value.PoolIndex = this.Indexs.pop()].Set(Value);
            else {
                var Node = new IndexPoolNode();
                Node.Identity = 0;
                Node.Set(Value);
                Value.PoolIndex = this.Nodes.length;
                this.Nodes.push(Node);
            }
        }
        /**
         * 移除保存对象
         */
        static Pop(Value: IIndexPool) {
            this.Nodes[Value.PoolIndex]
            var Node = this.Nodes[Value.PoolIndex];
            if (Node && Node.Pop(Value)) this.Indexs.push(Value.PoolIndex);
        }
        /**
         * 获取保存对象
         */
        static Get(Index: number, Identity: number): IIndexPool {
            var Node = this.Nodes[Index];
            return Node ? Node.Get(Identity) : null;
        }
        /**
         * 获取保存对象调用转字符串，用于回调传参字符串
         */
        static ToString(Value: IIndexPool) {
            return 'AutoCSer.IndexPool.Get(' + Value.PoolIndex + ',' + Value.PoolIdentity + ')';
        }
    }
    export class LoadJs {
        private OnLoad: Function;
        private OnError: Function;
        private Script: HTMLScriptElement;
        private LoadFunction: (Event: Event) => any;
        private ErrorFunction: (Event: Event) => any;
        /**
         * 加载 script 控件
         * @param OnLoad onload 事件
         * @param OnError onerror 事件
         */
        constructor(Script: HTMLScriptElement, OnLoad: Function = null, OnError: Function = null) {
            this.OnLoad = OnLoad;
            this.OnError = OnError;
            this.LoadFunction = Pub.ThisFunction(this, this.OnLoadJs) as (Event: Event) => any;
            this.ErrorFunction = Pub.ThisFunction(this, this.OnErrorJs) as (Event: Event) => any;
            (this.Script = Script).onload = this.LoadFunction;
            Script.onerror = this.ErrorFunction;
            Pub.DocumentHead.appendChild(Script);
        }
        private OnLoadJs(Event: Event) {
            if (this.OnLoad) this.OnLoad({ State : 1, Result: { ErrorEvent: Event } } as IHttpRequestReturn);
            Pub.DocumentHead.removeChild(this.Script);
        }
        private OnErrorJs(Event: Event) {
            if (this.OnError) this.OnError({ State: 0xff, Result: { ErrorEvent: Event } } as IHttpRequestReturn);
            Pub.DocumentHead.removeChild(this.Script);
        }
    }
    export class Events {
        private Functions: Function[];
        /**
         * 事件调用包装函数，应用层调用该函数触发事件函数调用
         */
        Function: Function;
        constructor() {
            this.Functions = [];
            this.Function = Pub.ThisFunction(this, this.Call);
        }
        private Call() {
            for (var Argument = Pub.ToArray(arguments), Index = 0; Index - this.Functions.length; this.Functions[Index++].apply(null, Argument));
        }
        Add(Function: Function): Events {
            if (Function && this.Functions.IndexOfValue(Function) < 0) this.Functions.push(Function);
            return this;
        }
        /**
         * 将指定事件的所有函数添加到当前事件
         */
        AddEvent(Event: Events): Events {
            if (Event) for (var Index = 0, Functions = Event.Functions; Index - Functions.length; this.Add(Functions[Index++]));
            return this;
        }
        Remove(Function: Function): Events {
            if (Function) this.Functions.RemoveAt(this.Functions.IndexOfValue(Function));
            return this;
        }
        /**
         * 从当前事件中移除指定事件的所有函数
         */
        RemoveEvent(Event: Events): Events {
            if (Event) for (var Index = 0, Functions = Event.Get(); Index - Functions.length; this.Remove(Functions[Index++]));
            return this;
        }
        Clear(): Events {
            this.Functions.length = 0;
            return this;
        }
        /**
         * 获取所有事件函数
         */
        Get(): Function[] {
            return this.Functions;
        }
    }
    Pub.OnQueryEvents = new Events();
    export class HttpRequestQuery {
        CallBack: Function;
        Url: string;
        FormData: FormData;
        /**
         * 调用传参对象
         */
        Send: any;
        /**
         * 请求方法 GET / POST
         */
        Method: string;
        /**
         * XMLHttpRequest.open 调用认证参数
         */
        UserName: string;
        /**
         * XMLHttpRequest.open 调用认证参数
         */
        Password: string;
        /**
         * 请求是否追求随机参数
         */
        IsRandom: boolean;
        /**
         * 是否全局静态版本
         */
        IsStaticVersion: boolean;
        /**
         * 回调操作是否等待 AutoCSer 初始化加载完毕
         */
        IsOnLoad: boolean;
        /**
         * 获取错误回调的请求对象
         */
        ErrorRequest: HttpRequest;
        /**
         * @param Send 需要 JSON 序列化的传参对象
         * @param IsStaticVersion 是否全局静态版本
         */
        constructor(Url: string, Send: Object = null, CallBack: Function = null, IsStaticVersion = false) {
            this.Url = Url;
            this.Send = Send;
            this.CallBack = CallBack;
            this.IsStaticVersion = IsStaticVersion;
        }
        /**
         * 创建请求信息
         */
        ToQueryInfo(): HttpRequestQueryInfo {
            var Query = new HttpRequestQueryInfo(null);
            Query.CallBack = this.CallBack || HttpRequestQuery.NullCallBack;
            Query.Url = this.Url;
            Query.FormData = this.FormData;
            Query.Send = this.Send;
            Query.Method = this.Method;
            Query.UserName = this.UserName;
            Query.Password = this.Password;
            Query.IsRandom = this.IsRandom;
            Query.IsStaticVersion = this.IsStaticVersion;
            Query.IsOnLoad = this.IsOnLoad;
            return Query;
        }
        /**
         * 获取错误回调包装函数
         * @param HttpRequest 获取错误回调的请求对象
         */
        GetOnError(HttpRequest: HttpRequest) {
            if (this.CallBack || HttpRequest) {
                this.ErrorRequest = HttpRequest;
                return Pub.ThisFunction(this, this.OnError);
            }
            return null;
        }
        /**
         * 错误回调
         */
        private OnError(Value: IHttpRequestReturn) {
            if (this.ErrorRequest != null) {
                this.ErrorRequest.NextRequest();
                this.ErrorRequest = null;
            }
            if (this.CallBack != null) {
                Value.ErrorRequest = this;
                this.CallBack(Value);
            }
        }
        /**
         * 默认缺省回调函数
         */
        private static NullCallBack() { }
    }
    export class HttpRequestQueryInfo extends HttpRequestQuery implements IIndexPool {
        /**
         * 对象池索引
         */
        PoolIndex: number;
        /**
         * 对象池索引标识
         */
        PoolIdentity: number;
        /**
         * 调用传参字符串
         */
        SendString: string;
        /**
         * 请求头部
         */
        Header: { [key: string]: string; };
        /**
         * script 加载模式重试次数
         */
        RetryCount: number;
        /**
         * 请求对象
         */
        Request: XMLHttpRequest;
        /**
         * 移除保存对象
         */
        PopIndexPool() {
            IndexPool.Pop(this);
        }
    }
    export class HttpRequest {
        /**
         * 是否已经触发请求
         */
        private Requesting: boolean;
        /**
         * 请求队列下一个写入位置
         */
        private WriteOrder: number;
        /**
         * 请求队列上一个读取位置
         */
        private ReadOrder: number;
        /**
         * 请求队列
         */
        private Queue: HttpRequestQueryInfo[];
        /**
         * 获取 XMLHttpRequest.responseText 以后，触发回调之前的操作处理
         */
        private OnResponse: Events;
        /**
         * 当前触发的请求
         */
        private ReadXMLHttpRequest: XMLHttpRequest;
        /**
         * onreadystatechange
         */
        private OnReadyStateChangeFunction: (Event: ProgressEvent) => any;
        /**
         * @param OnResponse 获取 XMLHttpRequest.responseText 以后，触发回调之前的操作处理
         */
        constructor(OnResponse: Function = null) {
            this.Requesting = false;
            this.WriteOrder = 0;
            this.ReadOrder = -1;
            this.Queue = [];
            this.OnResponse = new Events().Add(OnResponse);
            this.OnReadyStateChangeFunction = Pub.ThisFunction(this, this.OnReadyStateChange) as (Event: ProgressEvent) => any;
        }
        /**
         * 触发请求
         */
        Request(Request: HttpRequestQueryInfo) {
            if (Request.Send && !Request.FormData) {
                Request.SendString = Pub.ToJson(Request.Send);
                if (Request.SendString === '{}') Request.SendString = '';
            }
            this.Queue[this.WriteOrder++] = Request;
            if (!this.Requesting) {
                this.Requesting = true;
                this.MakeXMLHttpRequest();
            }
        }
        /**
         * 全局 AJAX 回调函数
         */
        CallBack() {
            var Request = this.Queue[this.ReadOrder];
            this.NextRequest();
            if (Request.CallBack) {
                if (Request.IsOnLoad) Pub.OnLoad(Pub.ThisFunction(this, this.OnLoad, [Request.CallBack, Pub.ToArray(arguments)]), null, true);
                else Request.CallBack.apply(null, Pub.ToArray(arguments));
            }
        }
        /**
         * 等待 AutoCSer 初始化加载完毕以后触发回调操作
         */
        private OnLoad(CallBack: Function, Arguments: any[]) {
            CallBack.apply(null, Arguments);
        }
        /**
         * onreadystatechange
         */
        private OnReadyStateChange(Event: ProgressEvent): any {
            var Request = this.ReadXMLHttpRequest;
            if (Request.readyState == 4) {
                var Query = this.Queue[this.ReadOrder], IsError = true;
                try {
                    if (Request.status == 200 || Request.status == 304) {
                        var Text = Request.responseText;
                        this.OnResponse.Function(Text);
                        if (Query.CallBack) {
                            eval(Text);
                            IsError = false;
                        }
                        else {
                            this.NextRequest();
                            eval(Text);
                            IsError = false;
                        }
                    }
                    else {
                        this.NextRequest();
                        HttpRequest.SendError(Query);
                    }
                }
                finally {
                    IndexPool.Pop(Query);
                    if (IsError && Query.CallBack) Query.CallBack({ Result: null, State: 0xff, ErrorRequest: Query } as IHttpRequestReturn);
                }
            }
        }
        static SendError(Query: HttpRequestQueryInfo) { }
        /**
         * 触发请求队列下一个请求
         */
        NextRequest() {
            if (this.ReadOrder === this.WriteOrder - 1) {
                this.WriteOrder = 0;
                this.ReadOrder = -1;
                this.Requesting = false;
            }
            else this.MakeXMLHttpRequest();
        }
        /**
         * 触发请求
         */
        private MakeXMLHttpRequest() {
            var Request = this.ReadXMLHttpRequest = HttpRequest.CreateRequest(), Info = this.Queue[++this.ReadOrder];
            var Url = Info.Url;
            if (Info.Method == null || Info.FormData) Info.Method = 'POST';
            if (Info.SendString && !Info.FormData) {
                if (Info.Method === 'GET') {
                    Url += (Url.indexOf('?') + 1 ? '&' : '?') + '__JSONQUERYNAME__=' + encodeURI(Info.SendString);
                    Info.SendString = null;
                }
                else Info.SendString = Info.SendString.replace(/\xA0/g, ' ');
            }
            if (Info.IsRandom) Url += (Url.indexOf('?') + 1 ? '&' : '?') + 't=' + (new Date).getTime();
            else if (Info.IsStaticVersion && Info.Method === 'GET') Url += (Url.indexOf('?') + 1 ? '&' : '?') + '__VERSIONQUERYNAME__=' + Pub.Version;
            Info.Request = Request;
            if (!Pub.IE && Info.Method === 'GET' && !Info.UserName && !Info.IsOnLoad) {
                Info.RetryCount = 2;
                Pub.AppendJs(Url, Pub.Charset, Pub.ThisFunction(Info, Info.PopIndexPool), (Pub.AjaxAppendJs = Info).GetOnError(this));
            }
            else {
                Request.onreadystatechange = this.OnReadyStateChangeFunction;
                if (Info.UserName == null || Info.UserName === '') Request.open(Info.Method, Url, true);
                else Request.open(Info.Method, Url, true, Info.UserName, Info.Password);
                if (Info.Header) {
                    for (var Name in Info.Header) Request.setRequestHeader(Name, Info.Header[Name]);
                }
                else if (Info.Method === 'POST' && !Info.FormData) Request.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
                Request.send(Info.FormData || Info.SendString);
            }
        }
        /**
         * Internet Explorer AJAX 组件名称集合
         */
        private static MicrosoftXmlHttps: string[];
        /**
         * 创建 AJAX 请求
         */
        static CreateRequest(): XMLHttpRequest {
            if (Pub.IE) {
                for (var Index = HttpRequest.MicrosoftXmlHttps.length; Index;) {
                    try {
                        return new ActiveXObject(HttpRequest.MicrosoftXmlHttps[--Index]);
                    }
                    catch (e) { }
                }
            }
            else if (window['XMLHttpRequest']) return new XMLHttpRequest;
            AutoCSer.Pub.Alert('你的浏览器不支持服务器请求,请升级您的浏览器！');
            return null;
        }
        /**
         * 触发 POST 请求
         */
        static PostQuery(HttpRequestQuery: HttpRequestQuery) {
            HttpRequestQuery.Method = 'POST';
            var Query = HttpRequestQuery.ToQueryInfo(), Request = new HttpRequest;
            IndexPool.Push(Query);
            Query.Url += '?__CALLBACKQUERYNAME__=' + encodeURI(IndexPool.ToString(Query)) + '.CallBack';
            Request.Request(Query);
        }
        /**
         * 触发 POST 请求
         */
        static Post(Url: string, Send: Object = null, CallBack: Function = null) {
            this.PostQuery(new HttpRequestQuery(Url, Send, CallBack));
        }
        /**
         * 触发 GET 请求
         */
        static GetQuery(HttpRequestQuery: HttpRequestQuery) {
            HttpRequestQuery.Method = 'GET';
            var Query = HttpRequestQuery.ToQueryInfo();
            if (!Pub.IE && !HttpRequestQuery.IsRandom) {
                Query.IsRandom = false;
                Query.Url += '?__CALLBACKQUERYNAME__=AutoCSer.Pub.AjaxCallBack';
                this.AjaxGetRequest.Request(Query);
                return;
            }
            IndexPool.Push(Query);
            if (!Query.IsStaticVersion) Query.IsRandom = true;
            Query.Url += '?__CALLBACKQUERYNAME__=' + encodeURI(IndexPool.ToString(Query)) + '.CallBack';
            (new HttpRequest).Request(Query);
        }
        /**
         * 触发 GET 请求
         */
        static Get(Url: string, Send: Object = null, CallBack: Function = null, IsStaticVersion = false) {
            this.GetQuery(new HttpRequestQuery(Url, Send, CallBack, IsStaticVersion));
        }
        /**
         * 全局 AJAX 回调函数的 this 对象
         */
        static AjaxGetRequest: HttpRequest;
        /**
         * AutoCSer AJAX 组件初始化
         */
        static Load() {
            //['Microsoft.XMLHTTP','MSXML2.XMLHTTP','MSXML2.XMLHTTP.3.0','MSXML2.XMLHTTP.4.0','MSXML2.XMLHTTP.5.0']
            if (Pub.IE) this.MicrosoftXmlHttps = ['Microsoft.XMLHTTP', 'Msxml2.XMLHTTP'];
            Pub.AjaxCallBack = Pub.ThisFunction(this.AjaxGetRequest = new HttpRequest, this.AjaxGetRequest.CallBack);
        }
    }
    export interface IHttpRequestReturn {
        /**
         * 返回值状态，1 表示成功，0xff 表示客户端请求失败
         */
        State: number;
        /**
         * 请求返回数据
         */
        Result: any;
        /**
         * 失败的请求信息
         */
        ErrorRequest: HttpRequestQuery;
    }
    export interface IPointer {
        Left: number;
        Top: number;
    }
    export class HtmlElement {
        /**
         * 查询字符串 #id | *name | .className | /tagName | :type | ?css | @value
         */
        private FilterString: string;
        /**
         * 查询结果控件集合
         */
        private Elements: HTMLElement[];
        /**
         * 被查询控件
         */
        private Parent: HTMLElement;
        /**
         * 查询字符串解析后的查询函数
         */
        private Filter: (Parent: HTMLElement, HtmlElement: HtmlElement) => void;
        /**
         * 查询字符串当前解析位置
         */
        private FilterIndex: number;
        /**
         * 查询字符与查询函数关系对象
         */
        private FilterCreator: { [key: number]: Function; };
        /**
         * 查询字符串解析创建查询函数字符串
         */
        private FilterBuilder: string[];
        /**
         * 控件查询
         * @param Value 查询字符串 #id | *name | .className | /tagName | :type | ?css | @value
         * @param Parent 被查询控件 HTMLElement ，默认为 document.body ，如果传参为 HtmlElement 则为第一个控件
         */
        constructor(Value: any, Parent: any) {
            if (typeof (Value) == 'string') {
                this.FilterString = Value;
                this.Parent = Parent ? (Parent instanceof HtmlElement ? (Parent as HtmlElement).Element0() : Parent as HTMLElement) : document.body;
            }
            else if (Value) {
                if (Value instanceof HtmlElement) {
                    this.FilterString = Value.FilterString;
                    this.Parent = Value.Parent;
                    this.Elements = Value.Elements;
                }
                else this.Elements = Value instanceof Array ? Value as HTMLElement[] : [Value as HTMLElement];
            }
            else this.Elements = [];
        }
        /**
         * 查询控件 id
         */
        FilterId() {
            var Id = this.FilterValue();
            this.FilterBuilder.push('function(Element,Value){if(Element==this.Parent?Value.IsParent(Element=document.getElementById("');
            this.FilterBuilder.push(Id);
            this.FilterBuilder.push('")):Element.id=="');
            this.FilterBuilder.push(Id);
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        /**
         * 查询控件 childNodes 标签名称（小写字母）
         */
        FilterChildTag() {
            if (this.FilterIndex === this.FilterString.length || this.FilterString.charCodeAt(this.FilterIndex) - 47) {
                var Name = this.FilterValue();
                this.FilterBuilder.push('function(Element,Value){for(var Elements=Element.childNodes,Index=0;Index-Elements.length;)if((Element=Elements[Index++])');
                if (Name) {
                    this.FilterBuilder.push('.tagName&&Element.tagName.toLowerCase()=="');
                    this.FilterBuilder.push(Name.toLowerCase());
                    this.FilterBuilder.push('"');
                }
                this.FilterBuilder.push(')(');
                this.FilterNext(true);
            }
            else {
                ++this.FilterIndex;
                this.FilterTag();
            }
        }
        /**
         * 查询控件标签名称
         */
        FilterTag() {
            var Name = this.FilterValue();
            if (Name) this.FilterChildren('tagName', Name);
            else {
                this.FilterChildren();
                this.FilterBuilder.push('if(Element=Childs[Index++])(');
                this.FilterNext(true);
            }
        }
        /**
         * 查询控件 childNodes 指定属性名称（小写字母）
         */
        FilterChildren(Name: string = null, Value = '') {
            this.FilterBuilder.push('function(Element,Value){var Elements=[],ElementIndex=-1;while(ElementIndex-Elements.length)for(var Childs=ElementIndex+1?Elements[ElementIndex++].childNodes:[arguments[++ElementIndex]],Index=0;Index-Childs.length;Elements.push(Element))');
            if (Name) {
                if (!Value) Value = this.FilterValue();
                this.FilterBuilder.push('if((Element=Childs[Index++]).');
                this.FilterBuilder.push(Name);
                this.FilterBuilder.push('&&Element.');
                this.FilterBuilder.push(Name);
                this.FilterBuilder.push('.toLowerCase()');
                this.FilterBuilder.push('=="');

                this.FilterBuilder.push(Value.toLowerCase());
                this.FilterBuilder.push('")(');
                this.FilterNext(true);
            }
        }
        /**
         * 查询控件 className 以空格符分割字符串
         */
        FilterClass() {
            this.FilterChildren();
            this.FilterBuilder.push('if((Element=Childs[Index++]).className&&Element.className.toString().split(" ").IndexOfValue("');
            this.FilterBuilder.push(this.FilterValue());
            this.FilterBuilder.push('")+1)(');
            this.FilterNext(true);
        }
        /**
         * 查询控件属性值，查询参数名称与匹配值用 = 分割，不存在 = 时仅判断属性名称是否存在
         */
        FilterAttribute() {
            this.FilterChildren();
            var Value = this.FilterValue().split('=');
            if (Value.length == 1) {
                this.FilterBuilder.push('if(AutoCSer.HtmlElement.$IsAttribute(Element=Childs[Index++],"');
                this.FilterBuilder.push(Value[0]);
                this.FilterBuilder.push('"))(');
            }
            else {
                this.FilterBuilder.push('if(AutoCSer.HtmlElement.$Attribute(Element=Childs[Index++],"');
                this.FilterBuilder.push(Value[0]);
                this.FilterBuilder.push('")=="');
                this.FilterBuilder.push(Value[1]);
                this.FilterBuilder.push('")(');
            }
            this.FilterNext(true);
        }
        /**
         * 查询控件 name
         */
        FilterName() {
            this.FilterChildren();
            this.FilterBuilder.push('if(AutoCSer.HtmlElement.$Attribute(Element=Childs[Index++],"name")=="');
            this.FilterBuilder.push(this.FilterValue());
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        /**
         * 查询控件 style，查询参数名称与匹配值用 = 分割
         */
        FilterCss() {
            this.FilterChildren();
            var Value = this.FilterValue().split('=');
            this.FilterBuilder.push('if(AutoCSer.HtmlElement.$GetStyle(Element=Childs[Index++],"');
            this.FilterBuilder.push(Value[0]);
            this.FilterBuilder.push('")=="');
            this.FilterBuilder.push(Value[1]);
            this.FilterBuilder.push('")(');
            this.FilterNext(true);
        }
        /**
         * 获取下一个查询参数值
         */
        FilterValue(): string {
            var Index = this.FilterIndex;
            while (this.FilterIndex !== this.FilterString.length && !this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)])++this.FilterIndex;
            return this.FilterString.substring(Index, this.FilterIndex);
        }
        /**
         * 解析下一个查询函数
         */
        FilterNext(IsEnd: boolean) {
            if (this.FilterIndex !== this.FilterString.length) {
                var Creator = this.FilterCreator[this.FilterString.charCodeAt(this.FilterIndex)];
                if (Creator) {
                    ++this.FilterIndex;
                    Creator();
                }
                else this.FilterTag();
                if (IsEnd) this.FilterBuilder.push(')(Element,Value);}');
            }
            else if (this.FilterBuilder.length) this.FilterBuilder.push('Value.Elements.push)(Element);}');
        }
        /**
         * 查询函数缓存，关键字为 查询字符串
         */
        private static FilterCache: { [key: string]: (Parent: HTMLElement, HtmlElement: HtmlElement) => void; } = {};
        /**
         * 获取查询结果控件集合
         */
        GetElements() {
            if (this.Elements) return this.Elements;
            if (!this.Filter) {
                var Filter = this.FilterString ? HtmlElement.FilterCache[this.FilterString] : HtmlElement.NullFilter;
                if (!Filter) {
                    this.FilterIndex = 0;
                    //                         #id                                        *name                                        .className                                    /tagName                                         :type                                                      ?css                                        @value
                    this.FilterCreator = { 35: Pub.ThisFunction(this, this.FilterId), 42: Pub.ThisFunction(this, this.FilterName), 46: Pub.ThisFunction(this, this.FilterClass), 47: Pub.ThisFunction(this, this.FilterChildTag), 58: Pub.ThisFunction(this, this.FilterChildren, ['type']), 63: Pub.ThisFunction(this, this.FilterCss), 64: Pub.ThisFunction(this, this.FilterAttribute) };
                    this.FilterBuilder = [];
                    this.FilterNext(false);
                    eval('Filter=' + this.FilterBuilder.join('') + ';');
                    HtmlElement.FilterCache[this.FilterString] = Filter;
                    this.FilterBuilder = this.FilterCreator = null;
                }
                this.Filter = Filter;
            }
            this.Elements = [];
            this.Filter(this.Parent, this);
            return this.Elements;
        }
        /**
         * 空查询函数缓存
         */
        static NullFilter(Parent: HTMLElement, HtmlElement: HtmlElement) { }
        /**
         * 循环被查询控件是否指定控件的上层节点
         */
        IsParent(Element: HTMLElement): boolean {
            return !this.Parent || (Element && HtmlElement.$IsParent(Element, this.Parent));
        }
        /**
         * 获取查询结果控件集合的第一个控件值
         */
        Element0(): HTMLElement {
            return this.GetElements()[0];
        }
        /**
         * 控件添加事件函数
         * @param Name 使用逗号,分给的事件名称，可以不包含 on 前缀
         * @param Value 绑定的事件执行函数
         * @param Internet Explorer 浏览器事件默认为 false 表示在冒泡阶段执行，其它默认为 true 表示在捕获阶段执行
         */
        AddEvent(Name: string, Value: any, IsStop: boolean = !Pub.IE): HtmlElement {
            return this.Event(Name, Value, IsStop, '$AddEvent');
        }
        /**
         * 控件删除事件函数
         * @param Name 使用逗号,分给的事件名称，可以不包含 on 前缀
         * @param Value 绑定的事件执行函数
         * @param Internet Explorer 浏览器事件默认为 false 表示在冒泡阶段执行，其它默认为 true 表示在捕获阶段执行
         */
        DeleteEvent(Name: string, Value: any, IsStop: boolean = !Pub.IE): HtmlElement {
            return this.Event(Name, Value, IsStop, '$DeleteEvent');
        }
        /**
         * 控件事件函数操作
         * @param Name 使用逗号,分给的事件名称，可以不包含 on 前缀
         * @param Value 绑定的事件执行函数
         * @param Internet Explorer 浏览器事件默认为 false 表示在冒泡阶段执行，其它默认为 true 表示在捕获阶段执行
         * @param Internet CallName $AddEvent / $DeleteEvent
         */
        private Event(Name: string, Value: any, IsStop: boolean, CallName: string) {
            var Elements = this.GetElements();
            if (Elements.length) {
                var Names = Name.split(',');
                for (var Index = 0; Index - Elements.length; HtmlElement[CallName](Elements[Index++], Names, Value, IsStop));
            }
            return this;
        }
        /**
         * 获取查询结果第一个控件的 value，比如 input.value 或者 select.options[select.selectedIndex].value
         */
        Value0(): string {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$GetValue(Elements[0]) : null;
        }
        /**
         * 设置控件 value，比如设置 input.value 或者设置匹配值的 select.selectedIndex
         */
        Value(Value: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetValue(Elements[Index++], Value));
            return this;
        }
        /**
         * 获取查询结果第一个控件指定属性名称的属性值
         * @param DefaultValue 不存在控件时返回的默认值
         */
        Get0<T>(Name: string, DefaultValue: T = null): any {
            var Elements = this.GetElements();
            return Elements.length ? Elements[0][Name] : DefaultValue;
        }
        /**
         * 获取查询结果第一个控件的 id
         */
        Id0(): string {
            return this.Get0<string>('id');
        }
        /**
         * 获取查询结果第一个控件的 innerHTML
         */
        Html0(): string {
            return this.Get0<string>('innerHTML');
        }
        /**
         * 获取查询结果第一个控件的标签名称 tagName
         */
        TagName0(): string {
            return this.Get0<string>('tagName');
        }
        /**
         * 获取查询结果第一个控件的滚动条位置 scrollHeight
         * @param DefaultHeight 不存在控件时返回的默认值
         */
        ScrollHeight0(DefaultHeight = 0) {
            return this.Get0('scrollHeight', DefaultHeight);
        }
        /**
         * 取查询结果第一个控件的属性值，不存在属性名称时查找 attributes[Name]
         */
        Attribute0(Name: string): string {
            var Elements = this.GetElements();
            if (Elements.length) return HtmlElement.$Attribute(Elements[0], Name);
        }
        /**
         * 取查询结果第一个控件的 name
         */
        Name0(): string {
            return this.Attribute0('name');
        }
        /**
         * 查询结果第一个控件的调用
         */
        private GetCall0<T>(CallName: string): T {
            var Elements = this.GetElements();
            if (Elements.length) return HtmlElement[CallName](Elements[0]);
        }
        /**
         * 获取查询结果第一个控件的 textContent / innerText / nodeValue
         */
        Text0(): string {
            return this.GetCall0<string>('$GetText');
        }
        /**
         * 设置查询结果控件集合集合的文本属性值 textContent / innerText / nodeValue
         */
        Text(Text: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetText(Elements[Index++], Text));
            return this;
        }
        /**
         * 获取查询结果第一个控件的样式 style 对象
         */
        Css0(): CSSStyleDeclaration {
            return this.GetCall0<CSSStyleDeclaration>('$Css');
        }
        /**
         * 获取查询结果第一个控件的宽度像素值 offsetWidth
         */
        Width0(): number {
            return this.GetCall0<number>('$Width');
        }
        /**
         * 获取查询结果第一个控件的高度像素值 offsetHeight
         */
        Height0(): number {
            return this.GetCall0<number>('$Height');
        }
        /**
         * 获取查询结果第一个控件的透明度 0-100
         */
        Opacity0(): number {
            return this.GetCall0<number>('$GetOpacity');
        }
        /**
         * 获取查询结果第一个控件的绝对坐标位置
         */
        XY0(): IPointer {
            return this.GetCall0<IPointer>('$XY');
        }
        /**
         * 获取查询结果第一个控件的样式属性值 style.Name
         */
        Style0(Name: string): string {
            var Css = this.Css0();
            return Css ? Css[Name] : null;
        }
        /**
         * 获取查询结果第一个控件的父节点 parentNode / parentElement
         */
        Parent0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$ParentElement(Elements[0])) : null;
        }
        /**
         * 获取查询结果第一个控件的后一个控件 nextSibling
         */
        Next0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$NextElement(Elements[0])) : null;
        }
        /**
         * 获取查询结果第一个控件的前一个控件 previousSibling
         */
        Previous0(): HtmlElement {
            var Elements = this.GetElements();
            return Elements.length ? HtmlElement.$(HtmlElement.$PreviousElement(Elements[0])) : null;
        }
        /**
         * 将查询结果第一个控件在 DOM 中替换为指定控件
         * @param Element 替换位置新添加的控件
         */
        Replace0(Element: HTMLElement): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) HtmlElement.$ParentElement(Elements[0]).replaceChild(Element, Elements[0]);
            return this;
        }
        /**
         * 查询结果控件集合设置属性值
         */
        Set(Name: string, Value: any): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; Elements[Index++][Name] = Value);
            return this;
        }
        /**
         * 查询结果控件集合设置 innerHTML
         * @param IsToHtml 默认为 false 表示不需要转义，否则将调用 ToHTML() 转义处理
         */
        Html(Html: string, IsToHtml = false): HtmlElement {
            return this.Set('innerHTML', IsToHtml ? Html.ToHTML() : Html);
        }
        /**
         * 控件移动到指定父节点下面
         */
        To(Parent: any = document.body): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) {
                if (Parent instanceof HtmlElement) Parent = (Parent as HtmlElement).Element0();
                for (var Index = -1; ++Index - Elements.length;) {
                    if (HtmlElement.$ParentElement(Elements[Index]) != Parent) Parent.appendChild(Elements[Index]);
                }
            }
            return this;
        }
        /**
         * 获取查询结果控件集合的子节点集合 childNodes
         */
        Child(): HtmlElement {
            for (var Nodes: Node[] = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
                var Childs = Elements[Index].childNodes;
                if (Childs) for (var NodeIndex = 0; NodeIndex !== Childs.length; Nodes.push(Childs[NodeIndex++]));
            }
            return new HtmlElement(Nodes, null);
        }
        /**
         * 获取查询结果控件集合的所有子孙节点集合
         * @param IsChild 判断节点是否符合条件
         */
        Childs(IsChild: (HTMLElement) => boolean = null) {
            for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; ++Index) {
                var Values = HtmlElement.$ChildElements(Elements[Index], IsChild);
                if (Values) Value.push(Values)
            }
            return HtmlElement.$(Value.concat.apply([], Value));
        }
        /**
         * 将查询结果控件集合的插入到指定控件之前
         * @param Element 指定控件
         * @param Parent 指定控件的父节点，允许不传
         */
        InsertBefore(Element: HTMLElement, Parent: HTMLElement = null): HtmlElement {
            if (!Parent) Parent = HtmlElement.$ParentElement(Element);
            for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; Parent.insertBefore(Elements[Index++], Element));
            return this;
        }
        /**
         * 从 DOM 中移除查询结果控件集合
         */
        Delete(): HtmlElement {
            for (var Elements = this.GetElements(), Index = 0; Index !== Elements.length; HtmlElement.$Delete(Elements[Index++]));
            return this;
        }
        /**
         * 查询结果控件集合添加样式 class
         * @param Name 空格符分割的样式 class 名称集合
         */
        AddClass(Name: string): HtmlElement {
            return this.Class(Name, '$AddClass');
        }
        /**
         * 查询结果控件集合移除样式 class
         * @param Name 空格符分割的样式 class 名称集合
         */
        RemoveClass(Name: string): HtmlElement {
            return this.Class(Name, '$RemoveClass');
        }
        /**
         * 查询结果控件集合样式 class 操作
         * @param Name 空格符分割的样式 class 名称集合
         */
        private Class(Name: string, CallName: string): HtmlElement {
            if (Name) {
                var Elements = this.GetElements();
                if (Elements.length) {
                    for (var Index = 0, Names = Name.split(' '); Index !== Elements.length; HtmlElement[CallName](Elements[Index++], Names));
                }
            }
            return this;
        }
        /**
         * 查询结果控件集合设置样式 style 属性值
         */
        Style(Name: string, Value: any): HtmlElement {
            for (var Elements = this.GetElements(), Index = Elements.length; Index; Elements[--Index].style[Name] = Value);
            return this;
        }
        /**
         * 查询结果控件集合设置样式 style 属性值
         * @param Name 逗号,分割的样式属性名称
         */
        Styles(Name: string, Value: string): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) {
                for (var ElementIndex = 0, Names = Name.split(','); ElementIndex - Elements.length;) {
                    for (var Element = Elements[ElementIndex++], Index = 0; Index - Names.length; Element.style[Names[Index++]] = Value);
                }
            }
            return this;
        }
        /**
         * 查询结果控件集合设置显示样式 style.display 属性值
         * @param IsShow 字符串 string 直接为属性值，否则转逻辑值 true 为空字符串表示显示，false 为 none 表示不显示
         */
        Display(IsShow: any): HtmlElement {
            return this.Style('display', typeof (IsShow) == 'string' ? IsShow : (IsShow ? '' : 'none'));
        }
        /**
         * 查询结果控件集合设置禁用样式 style.disabled 属性值
         */
        Disabled(Value: boolean) {
            return this.Style('disabled', Value);
        }
        /**
         * 查询结果控件集合设置坐标样式 style.left 属性值
         * @param Value 左侧位置像素值
         */
        Left(Value: number): HtmlElement {
            return this.Style('left', Value + 'px');
        }
        /**
         * 查询结果控件集合设置坐标样式 style.top 属性值
         * @param Value 顶部位置像素值
         */
        Top(Value: number): HtmlElement {
            return this.Style('top', Value + 'px');
        }
        /**
         * 查询结果控件集合设置控件坐标绝对位置
         */
        ToXY(Left: number, Top: number) {
            var Elements = this.GetElements();
            if (Elements.length) {
                for (var Index = Elements.length; Index; HtmlElement.$ToXY(Elements[--Index], Left, Top));
            }
            return this;
        }
        /**
         * 查询结果控件集合设置光标样式 cursor
         */
        Cursor(Value: string) {
            return this.Style('cursor', Value);
        }
        /**
         *  查询结果控件集合设置控件的透明度
         * @param Value 0-100
         */
        Opacity(Value: number): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; HtmlElement.$SetOpacity(Elements[Index++], Value));
            return this;
        }
        /**
         *  在查询结果控件集合中查找第一个符合指定条件的控件
         */
        FirstElement(IsValue: (HTMLElement) => boolean): HTMLElement {
            return this.GetElements().First(IsValue);
        }
        /**
         *  获取查询结果控件集合的样式 style 属性值集合，数组索引与查询结果控件集合一致
         */
        CssArray(Name: string): any[] {
            for (var Value = [], Elements = this.GetElements(), Index = 0; Index - Elements.length; Value.push(HtmlElement.$Css(Elements[Index++])[Name]));
            return Value;
        }
        /**
         *  查询结果控件集合设置样式 style.zIndex 为置顶显示
         */
        TopIndex(): HtmlElement{
            if (this.CssArray('zIndex').MaxValue() != HtmlElement.ZIndex && this.Elements.length) this.Style('zIndex', ++HtmlElement.ZIndex);
            return this;
        }
        /**
         *  查询结果控件集合的第一个控件设置聚焦 focus()
         */
        Focus0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) Elements[0].focus();
            return this;
        }
        /**
         *  查询结果控件集合的第一个控件设置失焦 blur()
         */
        Blur0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) Elements[0].blur();
            return this;
        }
        /**
         * 查询结果控件集合的属性逻辑值取反
         * @param Name 需要取反操作的属性名称
         */
        ChangeBool(Name: string): HtmlElement {
            for (var Index = 0, Elements = this.GetElements(); Index - Elements.length; ++Index)	Elements[Index][Name] = !Elements[Index][Name];
            return this;
        }
        /**
         * 将滚动条 scrollTop 设置到查询结果控件集合的第一个控件顶部位置
         */
        ScrollTop0(): HtmlElement {
            var Elements = this.GetElements();
            if (Elements.length) HtmlElement.$SetScrollTop(HtmlElement.$XY(Elements[0]).Top);
            return this;
        }

        /**
         * 查找匹配控件
         */
        static $(Value: any, Parent: any = null): HtmlElement {
            return new HtmlElement(Value, Parent);
        }
        /**
         * 根据 id 查找控件 document.getElementById
         */
        static $Id(Id: string): HtmlElement {
            return this.$(document.getElementById(Id));
        }
        /**
         * 根据 id 查找控件 document.getElementById
         */
        static $IdElement(Id: string): HTMLElement {
            return document.getElementById(Id);
        }
        /**
         * 控件添加事件函数
         * @param Names 事件名称集合，可以不包含 on 前缀
         * @param Value 绑定的事件执行函数
         * @param Internet Explorer 浏览器事件默认为 false 表示在冒泡阶段执行，其它默认为 true 表示在捕获阶段执行
         */
        static $AddEvent(Element: HTMLElement, Names: string[], Value: any, IsStop = !Pub.IE) {
            var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
            this.$DeleteEvent(Element, Names, Value, IsStop);
            for (var Index = Names.length; --Index >= 0;) {
                var Name = Names[Index].toLowerCase();
                if (Pub.IE) {
                    if (Name.substring(0, 2) != 'on') Name = 'on' + Name;
                    if (IsBody && Name == 'onfocus') Name = 'onfocusin';
                    Element['attachEvent'](Name, Value);
                }
                else {
                    if (Name.substring(0, 2) === 'on') Name = Name.substring(2);
                    Element.addEventListener(Name, Value, IsStop);
                }
            }
        }
        /**
         * 控件删除事件函数
         * @param Names 事件名称集合，可以不包含 on 前缀
         * @param Value 绑定的事件执行函数
         * @param Internet Explorer 浏览器事件默认为 false 表示在冒泡阶段执行，其它默认为 true 表示在捕获阶段执行
         */
        static $DeleteEvent(Element: HTMLElement, Names: string[], Value: any, IsStop = !Pub.IE) {
            var IsBody = Pub.IE && (Element == document.body || (Element.tagName && Element.tagName.toLowerCase() == 'body'));
            for (var Index = Names.length; --Index >= 0;) {
                var Name = Names[Index].toLowerCase();
                if (Pub.IE) {
                    if (Name.substring(0, 2) != 'on') Name = 'on' + Name;
                    if (IsBody && Name == 'onfocus') Name = 'onfocusin';
                    Element['detachEvent'](Name, Value);
                }
                else {
                    if (Name.substring(0, 2) === 'on') Name = Name.substring(2);
                    Element.removeEventListener(Name, Value, IsStop);
                }
            }
        }
        /**
         * 循环判断指定节点是否存在指定上层节点
         */
        static $IsParent(Element: HTMLElement, Parent: HTMLElement): boolean {
            while (Element != Parent && Element) Element = (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode) as HTMLElement;
            return Element != null;
        }
        /**
         * 获取控件的父节点 parentNode / parentElement
         */
        static $ParentElement(Element: HTMLElement): HTMLElement {
            return (Pub.IE ? Element.parentElement || Element.parentNode : Element.parentNode) as HTMLElement;
        }
        /**
         * 获取控件的后一个控件 nextSibling
         */
        static $NextElement(Element: HTMLElement): HTMLElement {
            return Element ? Element.nextSibling as HTMLElement : null;
        }
        /**
         * 获取控件的前一个控件 previousSibling
         */
        static $PreviousElement(Element: HTMLElement): HTMLElement {
            return Element ? Element.previousSibling as HTMLElement : null;
        }
        /**
         * 移除控件
         */
        static $Delete(Element: HTMLElement, Parent: HTMLElement = null) {
            if (Element != null) (Parent || HtmlElement.$ParentElement(Element)).removeChild(Element);
        }
        /**
         * 查询符合指定条件的所有子孙节点集合
         */
        static $ChildElements(Element: HTMLElement, IsElement: (HTMLElement) => boolean = null): HTMLElement[] {
            var Value = [], Elements = [Element], ElementIndex = 0;
            while (ElementIndex < Elements.length) {
                for (var Childs = Elements[ElementIndex++].childNodes, Index = -1; ++Index - Childs.length;) {
                    if (IsElement == null || IsElement(Childs[Index])) Value.push(Childs[Index]);
                    Elements.push(Childs[Index] as HTMLElement);
                }
            }
            return Value;
        }
        /**
         * 创建指定标签名称的控件
         * @param Document 指定框架文档，默认为 document
         */
        static $Create(TagName: string, Document: Document = document): HtmlElement {
            return this.$(Document.createElement(TagName));
        }
        /**
         * 创建指定标签名称的控件
         * @param Document 指定框架文档，默认为 document
         */
        static $CreateElement(TagName: string, Document: Document = document): HTMLElement {
            return Document.createElement(TagName);
        }
        /**
         * 获取指定 id 控件的 value，比如 input.value 或者 select.options[select.selectedIndex].value
         */
        static $GetValueById(Id: string): string {
            return this.$GetValue(this.$IdElement(Id));
        }
        /**
         * 获取控件 value，比如 input.value 或者 select.options[select.selectedIndex].value
         */
        static $GetValue(Element: HTMLElement): string {
            if (Element) {
                if (Element.tagName.toLowerCase() === 'select') {
                    if ((Element as HTMLSelectElement).selectedIndex >= 0) return (Element as HTMLSelectElement).options[(Element as HTMLSelectElement).selectedIndex].value;
                    return null;
                }
                return (Element as Object as HTMLInputElement).value;
            }
        }
        /**
         * 设置控件 value，比如设置 input.value 或者设置匹配值的 select.selectedIndex
         */
        static $SetValue(Element: HTMLElement, Value: string){
            if (Element) {
                if (Element.tagName.toLowerCase() == 'select') {
                    for (var Index = (Element as HTMLSelectElement).options.length; Index;) {
                        if ((Element as HTMLSelectElement).options[--Index].value == Value) break;
                    }
                    (Element as HTMLSelectElement).selectedIndex = Index;
                }
                else (Element as HTMLInputElement).value = Value;
            }
        }
        /**
         * 设置指定 id 控件的 value，比如 input.value 或者 select.options[select.selectedIndex].value
         */
        static $SetValueById(Id: string, Value: string): HTMLElement{
            var Element = this.$IdElement(Id);
            this.$SetValue(Element, Value);
            return Element;
        }
        /**
         * 获取指定 id 控件的 value 并 parseInt 转换为整数返回值，比如 input.value 或者 select.options[select.selectedIndex].value
         * @param DefaultValue 控件值不存在时的默认返回值
         */
        static $IntById(Id: string, DefaultValue: number = null): number {
            var Value = this.$GetValueById(Id);
            return Value ? parseInt(Value, 10) : (DefaultValue || 0);
        }
        /**
         * 获取指定 id 控件的 value 并 parseFloat 转换为浮点数返回值，比如 input.value 或者 select.options[select.selectedIndex].value
         * @param DefaultValue 控件值不存在时的默认返回值
         */
        static $FloatById(Id: string, DefaultValue: number = null): number {
            var Value = this.$GetValueById(Id);
            return Value ? parseFloat(Value) : (DefaultValue || 0);
        }
        /**
         * 获取指定 id 控件的 checked
         */
        static $GetCheckedById(Id: string): boolean {
            var Element = this.$IdElement(Id);
            return Element && Element['checked'];
        }
        /**
         * 设置指定 id 控件的 checked
         */
        static $SetCheckedById(Id: string, Checked: boolean): HTMLElement {
            var Element = this.$IdElement(Id);
            if (Element) Element['checked'] = Checked;
            return Element;
        }
        /**
         * 获取控件 textContent / innerText / nodeValue
         */
        static $GetText(Element: HTMLElement): string {
            return Pub.IE ? Element.nodeType == 3 ? Element.nodeValue : Element.innerText : Element.textContent;
        }
        /**
         * 设置控件文本属性值 textContent / innerText / nodeValue
         */
        static $SetText(Element: HTMLElement, Text: string): HTMLElement {
            if (Pub.IE) {
                if (Element.nodeType == 3) Element.nodeValue = Text;
                else Element.innerText = Text;
            }
            else Element.textContent = Text;
            return Element;
        }
        /**
         * 控件添加样式 class
         * @param Names 样式名称集合
         */
        static $AddClass(Element: HTMLElement, Names: string[]) {
            if (Names) {
                if (Element.classList) {
                    for (var Index = Names.length; Index ; Element.classList.add(Names[--Index]));
                }
                else {
                    var OldName = Element.className;
                    if (OldName) {
                        for (var Index = Names.length, OldNames = OldName.split(' '); --Index >= 0;)	if (OldNames.IndexOfValue(Names[Index]) < 0) OldNames.push(Names[Index]);
                        Names = OldNames;
                    }
                    Element.className = Names.join(' ');
                }
            }
        }
        /**
         * 控件移除样式 class
         * @param Names 样式名称集合
         */
        static $RemoveClass(Element: HTMLElement, Names: string[]) {
            if (Names) {
                if (Element.classList) {
                    for (var Index = Names.length; Index; Element.classList.remove(Names[--Index]));
                }
                else {
                    var OldName = Element.className;
                    if (OldName) {
                        for (var Index = Names.length, OldNames = OldName.split(' '); Index; OldNames.RemoveAtEnd(OldNames.IndexOfValue(Names[--Index])));
                        Element.className = OldNames.length ? OldNames.join(' ') : '';
                    }
                }
            }
        }
        /**
         * 获取控件样式 style 对象
         */
        static $Css(Element: HTMLElement): CSSStyleDeclaration {
            return Pub.IE ? Element['currentStyle'] : document.defaultView.getComputedStyle(Element);
        }
        /**
         * 获取控件样式 style 的属性值
         */
        static $GetStyle(Element: HTMLElement, Name: string): string {
            var Css = this.$Css(Element);
            return Css ? Css[Name] : null;
        }
        /**
         * 设置控件样式 style 的属性值
         */
        static $SetStyle(Element: HTMLElement, Name: string, Value: any) {
            Element.style[Name] = Value;
        }
        /**
         * 获取控件属性值，属性值不存在时则返回样式 style 的属性值
         */
        static $AttributeOrStyle(Element: HTMLElement, Name: string ): string {
            return this.$Attribute(Element, Name) || this.$GetStyle(Element, Name);
        }
        /**
         * 获取控件的属性值，不存在属性名称时查找 attributes[Name]
         */
        static $Attribute(Element: HTMLElement, Name: string): string {
            var Value = Element[Name];
            return Value === undefined && Element.attributes && (Value = Element.attributes[Name]) ? Value.value : Value;
        }
        /**
         * 判断控件是否存在属性名称，属性名称不存在时则判断 attributes 是否存在属性名称 
         */
        static $IsAttribute(Element: HTMLElement, Name: string): boolean {
            return Element[Name] !== undefined || (Element.attributes != null && Element.attributes[Name] !== undefined);
        }
        /**
         * 根据属性名称查找控件，如果当前控件属性不匹配则继续匹配父节点控件直到顶层节点（不查找子节点）
         * @param Element 起始匹配控件
         * @param AttributeName 匹配属性名称
         * @param Value 默认为 null 表示仅匹配属性名称，否则需要匹配属性值
         */
        static $ParentName(Element: HTMLElement, AttributeName: string, Value: string = null): HTMLElement {
            if (Value == null) {
                while (Element && HtmlElement.$Attribute(Element, AttributeName) == null) Element = HtmlElement.$ParentElement(Element);
            }
            else while (Element && HtmlElement.$Attribute(Element, AttributeName) != Value) Element = HtmlElement.$ParentElement(Element);
            return Element;
        }
        /**
         * 计算控件的绝对坐标位置
         */
        static $XY(Element: HTMLElement): IPointer {
            for (var Left = 0, Top = 0; Element != null && Element != document.body; Element = Element.offsetParent as HTMLElement) {
                Left += Element.offsetLeft;
                Top += Element.offsetTop;
                if (Pub.IsFixed) {
                    var Css = this.$Css(Element), Transform = Css['transform'] || Css['-webkit-transform'];
                    if (Css['position'] == 'fixed') {
                        Left += this.$GetScrollLeft();
                        Top += this.$GetScrollTop();
                    }
                    var XY = this.$Transform(Transform);
                    if (XY) {
                        if (XY.Left) Left += XY.Left;
                        if (XY.Top) Top += XY.Top;
                    }
                }
                if (Pub.IsBorder) {
                    var Css = this.$Css(Element);
                    Left -= parseInt(0 + Css['border-left-width'], 10);
                    Top -= parseInt(0 + Css['border-top-width'], 10);
                    if (Pub.IsPadding) {
                        Left -= parseInt(0 + Css['padding-left'], 10);
                        Top -= parseInt(0 + Css['padding-top'], 10);
                    }
                }
            }
            return { Left: Left, Top: Top};
        }
        /**
         * 矩阵坐标转换
         */
        static $Transform_matrix(a, b, c, d, Left: number, Top: number): IPointer {
            return { Left: Left, Top: Top };
        }
        /**
         * 坐标转换
         */
        private static $Transform(Transform: string): IPointer {
            if(Transform && Transform.indexOf('matrix(') != -1) return eval('HtmlElement.$Transform_' + Transform) as IPointer;
        }
        /**
         * 设置控件坐标绝对位置
         */
        static $ToXY(Element: HTMLElement, Left: number, Top: number): HTMLElement{
            var Value = this.$XY(Element['offsetParent'] as HTMLElement);
            Element.style.left = (Left - Value.Left) + 'px';
            Element.style.top = (Top - Value.Top) + 'px';
            return Element;
        }
        /**
         * 获取控件的宽度像素值 offsetWidth
         */
        static $Width(Element: HTMLElement = null): number {
            if (Element == null) return window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
            return this.$Offset(Element, 'offsetWidth');
        }
        /**
         * 获取控件的高度像素值 offsetHeight
         */
        static $Height(Element: HTMLElement = null): number {
            if (Element == null) return window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
            return this.$Offset(Element, 'offsetHeight');
        }
        /**
         * 获取控件的数字属性值，当前控件不存在属性值时则循环获取该空间第一个子控件的属性值，不存在时返回 0
         */
        private static $Offset(Element: HTMLElement, Name: string): number {
            while (Element) {
                var Value = Element[Name];
                if (Value != null) return Value;
                var Elements = Element.children;
                if (Elements == null) return 0;
                Element = Elements[0] as HTMLElement;
            }
            return 0;
        }
        /**
         * 获取滚动条横坐标位置 scrollLeft
         */
        static $GetScrollLeft(): number {
            return Math.max(document.body.scrollLeft, document.documentElement.scrollLeft);
        }
        /**
         * 将滚动条横坐标 scrollLeft 设置到指定位置
         */
        static $SetScrollLeft(Left: number) {
            return document.body.scrollLeft = document.documentElement.scrollLeft = Left;
        }
        /**
         * 获取滚动条纵坐标位置 scrollTop
         */
        static $GetScrollTop(): number {
            return Math.max(document.body.scrollTop, document.documentElement.scrollTop);
        }
        /**
         * 将滚动条纵坐标 scrollTop 设置到指定位置
         */
        static $SetScrollTop(Top: number) {
            document.body.scrollTop = document.documentElement.scrollTop = Top;
        }
        /**
         * 获取滚动条纵坐标高度 scrollHeight
         */
        static $GetScrollHeight(): number {
            return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
        }
        /**
         * 获取滚动条横坐标宽度 scrollWidth
         */
        static $GetScrollWidth(): number {
            return Math.max(document.body.scrollWidth, document.documentElement.scrollWidth);
        }
        /**
         * 将滚动条纵坐标 scrollTop 设置到指定 id 控件的顶部位置
         */
        static $ScrollTopById(Id: string) {
            var Element = this.$IdElement(Id);
            if (Element) this.$SetScrollTop(this.$XY(Element).Top);
        }
        /**
         * 设置控件的透明度
         * @param Value 0-100
         */
        static $SetOpacity(Element: HTMLElement, Value: number) {
            if (Pub.IE) Element.style.filter = 'alpha(opacity=' + Value + ')';
            else Element.style.opacity = Element.style['MozOpacity'] = (Value / 100).toString();
        }
        /**
         * 获取控件的透明度 0-100
         */
        static $GetOpacity(Element: HTMLElement): number {
            if (Pub.IE) return Element.style.filter['alphas'].opacity;
            var Value = this.$Css(Element).opacity;
            return Value ? parseFloat(Value) * 100 : null;
        }
        /**
         * 获取名称 name 为指定值的控件集合
         */
        static $Name(Name: string, Element: HTMLElement = null): HtmlElement {
            if (Pub.IE) return HtmlElement.$(HtmlElement.$ChildElements(Element || document.body, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Name; }));
            return HtmlElement.$(Element ? HtmlElement.$ChildElements(Element, function (Element) { return HtmlElement.$Attribute(Element, 'name') == Name; }) : Pub.ToArray(document.getElementsByName(Name)));
        }
        /**
         * 黏贴文本到当前 textarea 选择区
         * @param IsAll 没有选择区时是否操作整个控件
         */
        static $Paste(Element: HTMLTextAreaElement, Text: string, IsAll: boolean) {
            if (Pub.IE) {
                var Selection = Element['document'].selection.createRange();
                if (IsAll && Selection.text == '') Element.value = Text;
                else {
                    Selection.text = Text;
                    Selection.moveStart('character', 0);
                    Selection.select();
                }
            }
            else {
                var StartIndex = Element.selectionStart, EndIndex = Element.selectionEnd;
                if (IsAll && StartIndex == EndIndex) {
                    Element.value = Text;
                    StartIndex = 0;
                }
                else {
                    var OldText = Element.value;
                    Element.value = OldText.substring(0, StartIndex) + Text + OldText.substring(EndIndex);
                }
                Element.selectionStart = Element.selectionEnd = StartIndex + Text.length;
            }
        }
        /**
         * 根据 select 控件 id 获取当前选择项控件 options[selectedIndex]
         */
        static $SelectedOption(Id: string): HTMLOptionElement {
            var Element = this.$IdElement(Id) as HTMLSelectElement;
            if (Element && Element.selectedIndex >= 0) return Element.options[Element.selectedIndex] as Object as HTMLOptionElement;
        }
        /**
         * 当前全局置顶显示的 style.zIndex 值
         */
        static ZIndex = 10000;
    }
    export class ViewData {
        /**
         * 视图数据
         */
        $Data: any;
        /**
         * 视图标记
         */
        $Mark: ViewMark;
        /**
         * 视图模板数据节点
         * @param View 数据视图模板
         * @param Data 视图数据
         */
        constructor(View: View, Data: Object) {
            this.$Mark = View.Marks[View.Marks.length - 1];
            this.$Data = Data;
        }
        /**
         * 根据成员名称获取视图模板数据节点
         * @param Name 成员名称
         */
        $GetMember(Name: any): ViewData {
            var Member = this[Name] as ViewData;
            if (Member && Member.$Mark) {
                Member.$Mark = Member.$Mark.GetMark(null);
                return Member;
            }
            return null;
        }
        /**
         * 设置视图数据
         * @param Data 新的视图数据
         */
        $Set(Data: any) {
            this.$Data = Data;
            this.$Mark.SetRefresh();
        }
    }
    class ViewExpression {
        /**
         * 表达式类型
         */
        Type: string;
        /**
         * 下一个节点
         */
        Next: ViewExpression;
        /**
         * 节点文本内容
         */
        Content: string;
        /**
         * 数字常量
         */
        Number: number;
        /**
         * 调用参数节点集合
         */
        Parameters: ViewExpression[];
        /**
         * 数据视图表达式
         * @param Type 表达式类型
         * @param Content 节点文本内容
         * @param Next 下一个节点
         * @param Parameters 调用参数节点集合
         * @param Number 数字常量
         */
        constructor(Type: string, Content: string = null, Next: ViewExpression = null, Parameters: ViewExpression[] = null, Number: number = 0) {
            this.Type = Type;
            this.Content = Content;
            this.Next = Next;
            this.Parameters = Parameters;
            this.Number = Number;
        }
        /**
         * 强制检查是否存在下一个节点
         */
         CheckNext(): ViewExpression {
            return this.Next != null ? this : null;
        }
        /**
         * 检查下一个节点是否合法
         */
        CheckNextNull(): ViewExpression  {
            if (this.Next) {
                if (!this.Next.Type) this.Next = null;
                return this;
            }
            return null;
        }
        /**
         * 设置下一个节点
         */
        SetNext(Next: ViewExpression): boolean {
            if (Next != null) {
                if (!Next.Type) this.Next = Next;
                return true;
            }
            return false;
        }
        /**
         * 空表达式节点
         */
        static Null: ViewExpression = new ViewExpression(null, null, null);
    }
    class ViewExpressionBuilder {
        /**
         * 表达式 HTML 代码
         */
        Html: string;
        /**
         * 当前解析内容开始位置
         */
        StartIndex: number;
        /**
         * 当前解析位置
         */
        Index: number;
        /**
         * 创建数据视图表达式
         * @param Html 表达式 HTML 代码
         */
        constructor(Html: string) {
            this.Html = Html.Trim();
            this.StartIndex = this.Index = 0;
        }
        /**
         * 取值解析
         */
        Value(): ViewExpression {
            do {
                var Code = this.Html.charCodeAt(this.Index);
                switch (Code & 7) {
                    case 0x21 & 7://!
                        if (Code == 0x21) return ++this.Index != this.Html.length ? new ViewExpression('!', null, this.Value()).CheckNext() : null;
                        break;
                    case 0x22 & 7://"
                        if (Code == 0x22) {
                            if (++this.Index - this.Html.length) {
                                this.StartIndex = this.Index;
                                var IsTransfer = 0;
                                do {
                                    if (!IsTransfer) {
                                        Code = this.Html.charCodeAt(this.Index);
                                        if (Code == 0x22) {
                                            var Content = this.Html.substring(this.StartIndex, this.Index).replace(/\\([\\"])/g, '$1');
                                            if (++this.Index == this.Html.length) return new ViewExpression('"', Content);
                                            return new ViewExpression('"', Content, this.MemberNext(2)).CheckNextNull();
                                        }
                                        //\
                                        if (Code == 0x5c) IsTransfer = 1;
                                    }
                                    else IsTransfer = 0;
                                }
                                while (++this.Index - this.Html.length);
                            }
                            return null;
                        }
                        break;
                    case 0x28 & 7://(
                        if (Code == 0x28) {
                            if (++this.Index - this.Html.length) {
                                var Parameter = this.Value();
                                if (Parameter && ++this.Index - this.Html.length) {
                                    do {
                                        Code = this.Html.charCodeAt(this.Index);
                                        //)
                                        if (Code == 0x29) {
                                            if (++this.Index == this.Html.length) return new ViewExpression(')', null, null, [Parameter]);
                                            return new ViewExpression(')', null, this.MemberNext(0), [Parameter]).CheckNextNull();
                                        }
                                        if (!this.CheckSpace(Code)) return null;
                                    }
                                    while (true);
                                }
                            }
                            return null;
                        }
                        break;
                    case 0x23 & 7://#
                        if (Code == 0x23) {
                            if (++this.Index == this.Html.length) return new ViewExpression('#', null, null, null);
                            if (ViewExpressionBuilder.IsMemberNameStart(this.Html.charCodeAt(this.Index))) {
                                this.StartIndex = this.Index;
                                return new ViewExpression('#', null, this.Member(-1), null).CheckNext();
                            }
                            return null;
                        }
                        //+
                        if (Code == 0x2b) return this.Signed(false);
                        break;
                    case 0x24 & 7://$
                        if (Code == 0x24) {
                            if (++this.Index != this.Html.length && ViewExpressionBuilder.IsMemberNameStart(this.Html.charCodeAt(this.Index))) {
                                this.StartIndex = this.Index;
                                return this.Member(-1);
                            }
                            return null;
                        }
                        break;
                    case 0x2d & 7://-
                        if (Code == 0x2d) return this.Signed(true);
                        break;
                    case 0x2e & 7://.
                        if (Code == 0x2e) {
                            this.StartIndex = this.Index;
                            if (++this.Index != this.Html.length) {
                                if (!ViewExpressionBuilder.IsNumber(this.Html.charCodeAt(this.Index))) {
                                    while (this.Html.charCodeAt(this.Index) == 0x2e) {
                                        if (++this.Index == this.Html.length) return null;
                                    }
                                    Code = this.Html.charCodeAt(this.Index);
                                    if (ViewExpressionBuilder.IsMemberNameStart(Code)) {
                                        var MemberDepth = this.Index - this.StartIndex;
                                        this.StartIndex = this.Index;
                                        return this.Member(MemberDepth);
                                    }
                                    return null;
                                }
                                return this.Float(false);
                            }
                            return null;
                        }
                        break;
                }
                if (ViewExpressionBuilder.IsMemberName(Code)) {
                    this.StartIndex = this.Index;
                    return ViewExpressionBuilder.IsNumber(Code) ? this.Number(Code, false) : this.Member(0);
                }
                if (!this.CheckSpace(Code)) return null;
            }
            while (true);
        }
        /**
         * 循环过滤空格，查找到非空格字符返回 true
         */
        private CheckSpace(Code: number): number {
            if (Code == 0x20) {
                do {
                    if (++this.Index == this.Html.length) return 0;
                }
                while (this.Html.charCodeAt(this.Index) == 0x20);
                return 1;
            }
            return 0;
        }
        /**
         * 成员解析
         * @param MemberDepth 成员回溯深度
         */
        private Member(MemberDepth: number): ViewExpression {
            while (++this.Index != this.Html.length) {
                if (!ViewExpressionBuilder.IsMemberName(this.Html.charCodeAt(this.Index)))
                {
                    if (!MemberDepth) {
                        switch (this.Index - this.StartIndex - 4) {
                            case 0:
                                if (this.Html.substring(this.StartIndex, this.Index) == 'true') return new ViewExpression('T');
                                break;
                            case 1:
                                if (this.Html.substring(this.StartIndex, this.Index) == 'false') return new ViewExpression('F');
                                break;
                        }
                    }
                    return new ViewExpression('M', this.Html.substring(this.StartIndex, this.Index), this.MemberNext(0), null, Math.max(MemberDepth, 0)).CheckNextNull();
                }
            }
            return new ViewExpression('M', this.Html.substring(this.StartIndex, this.Index), null, null, Math.max(MemberDepth, 0));
        }
        /**
         * 成员后续解析，调用者需要检查解析深度，返回 null 表示失败，返回 ValueExpression.Null 表示返回 null
         * @param ConstantType 0 表示非常量，1 表示数字，2 表示字符串
         */
        private MemberNext(ConstantType: number): ViewExpression {
            do {
                var Code = this.Html.charCodeAt(this.Index);
                switch (Code & 0xf) {
                    case 0x20 & 0xf:
                        if (!this.CheckSpace(Code)) return ViewExpression.Null;
                        break;
                    case 0x21 & 0xf://!
                        if (Code == 0x21) {
                            if (++this.Index != this.Html.length && this.Html.charCodeAt(this.Index) == 0x3d && ++this.Index != this.Html.length) return new ViewExpression('!=', null, this.Value()).CheckNext();
                            return null;
                        }
                        return ViewExpression.Null;
                    case 0x24 & 0xf://$
                        if (Code == 0x24) {
                            if (++this.Index != this.Html.length && ConstantType == 0) {
                                if (ViewExpressionBuilder.IsMemberNameStart(Code = this.Html.charCodeAt(this.Index))) {
                                    this.StartIndex = this.Index;
                                    return new ViewExpression('.', null, this.Member(-1)).CheckNext();
                                }
                                else if (Code == 0x26) {
                                    ++this.Index
                                    return new ViewExpression('$&');
                                }
                            }
                            return null;
                        }
                        return ViewExpression.Null;

                    case 0x25 & 0xf://%
                        if (Code == 0x25) {
                            return ++this.Index != this.Html.length && ConstantType != 2 ? new ViewExpression('%', null, this.Value()).CheckNext() : null;
                        }
                        return ViewExpression.Null;
                    case 0x26 & 0xf://&
                        if (Code == 0x26) {
                            if (++this.Index != this.Html.length) {
                                if (this.Html.charCodeAt(this.Index) == 0x26) return ++this.Index != this.Html.length ? new ViewExpression('&&', null, this.Value()).CheckNext() : null;
                                if (ConstantType != 2) return new ViewExpression('&', null, this.Value()).CheckNext();
                            }
                            return null;
                        }
                        return ViewExpression.Null;
                    case 0x28 & 0xf://(
                        if (Code == 0x28) return ++this.Index != this.Html.length && ConstantType == 0 ? this.Call('(', 0x29) : null;
                        return ViewExpression.Null;
                    case 0x2a & 0xf:
                        if (Code == 0x2a) {
                            return ++this.Index != this.Html.length && ConstantType != 2 ? new ViewExpression('*', null, this.Value()).CheckNext() : null;
                        }
                        return ViewExpression.Null;
                    case 0x2b & 0xf://+
                        if (Code == 0x2b) return ++this.Index != this.Html.length ? new ViewExpression('+', null, this.Value()).CheckNext() : null;
                        //[
                        if (Code == 0x5b) return ++this.Index != this.Html.length && ConstantType == 0 ? this.Call('[', 0x5d) : null;
                        return ViewExpression.Null;
                    case 0x3c & 0xf://<
                        if (Code == 0x3c) {
                            if (++this.Index != this.Html.length) {
                                switch (this.Html.charCodeAt(this.Index) - 0x3c) {
                                    case 0x3c - 0x3c://<
                                        if (++this.Index != this.Html.length && ConstantType != 2) return new ViewExpression('<<', null, this.Value()).CheckNext();
                                        break;
                                    case 0x3d - 0x3c://=
                                        if (++this.Index != this.Html.length) return new ViewExpression('<=', null, this.Value()).CheckNext();
                                        break;
                                    case 0x3e - 0x3c://>
                                        if (++this.Index != this.Html.length) return new ViewExpression('!=', null, this.Value()).CheckNext();
                                        break;
                                    default: return new ViewExpression('<', null, this.Value()).CheckNext();
                                }
                            }
                            return null;
                        }
                        //|
                        if (Code == 0x7c) {
                            if (++this.Index != this.Html.length) {
                                if (this.Html.charCodeAt(this.Index) == 0x7c) return ++this.Index != this.Html.length ? new ViewExpression('||', null, this.Value()).CheckNext() : null;
                                if (ConstantType != 2) return new ViewExpression('|', null, this.Value()).CheckNext();
                            }
                            return null;
                        }
                        return ViewExpression.Null;
                    case 0x3d & 0xf://=
                        if (Code == 0x3d) {
                            if (++this.Index != this.Html.length && this.Html.charCodeAt(this.Index) == 0x3d && ++this.Index != this.Html.length) return new ViewExpression('=', null, this.Value()).CheckNext();
                            return null;
                        }
                        //-
                        if (Code == 0x2d) return ++this.Index != this.Html.length && ConstantType != 2 ? new ViewExpression('-', null, this.Value()).CheckNext() : null;
                        return ViewExpression.Null;
                    case 0x2e & 0xf://.
                        if (Code == 0x2e) {
                            if (++this.Index != this.Html.length && ConstantType == 0 && ViewExpressionBuilder.IsMemberNameStart(this.Html.charCodeAt(this.Index))) {
                                this.StartIndex = this.Index;
                                return new ViewExpression('.', null, this.Member(-1)).CheckNext();
                            }
                            return null;
                        }
                        //>
                        if (Code == 0x3e) {
                            if (++this.Index != this.Html.length) {
                                switch (this.Html.charCodeAt(this.Index) - 0x3d) {
                                    case 0x3d - 0x3d://=
                                        if (++this.Index != this.Html.length) return new ViewExpression('>=', null, this.Value()).CheckNext();
                                        break;
                                    case 0x3e - 0x3d://>
                                        if (++this.Index != this.Html.length && ConstantType != 2) return new ViewExpression('>>', null, this.Value()).CheckNext();
                                        break;
                                    default: return new ViewExpression('>', null, this.Value()).CheckNext();
                                }
                            }
                            return null;
                        }
                        //^
                        if (Code == 0x5e) return ++this.Index != this.Html.length && ConstantType != 2 ? new ViewExpression('^', null, this.Value()).CheckNext() : null;
                        return ViewExpression.Null;
                    case 0x2f & 0xf:///
                        if (Code == 0x2f) return ++this.Index != this.Html.length && ConstantType != 2 ? new ViewExpression('/', null, this.Value()).CheckNext() : null;
                        //?
                        if (Code == 0x3f) {
                            if (++this.Index != this.Html.length) {
                                //.
                                if ((Code = this.Html.charCodeAt(this.Index)) == 0x2e) {
                                    if (!ConstantType && ++this.Index != this.Html.length && ViewExpressionBuilder.IsMemberNameStart(this.Html.charCodeAt(this.Index)))
                                    {
                                        this.StartIndex = this.Index;
                                        return new ViewExpression('?.', null, this.Member(-1)).CheckNext();
                                    }
                                    return null;
                                }
                                //?
                                if (Code == 0x3f) return ++this.Index != this.Html.length ? new ViewExpression('??', null, this.Value()).CheckNext() : null;
                                //[
                                if (Code == 0x5b) return !ConstantType && ++this.Index != this.Html.length ? this.Call('?[', 0x5d) : null;
                                var IfValue = this.Value();
                                if (IfValue && this.Index != this.Html.length) {
                                    do {
                                        //:
                                        if ((Code = this.Html.charCodeAt(this.Index)) == 0x3a) {
                                            if (++this.Index != this.Html.length) {
                                                var ElseValue = this.Value();
                                                if (ElseValue) return new ViewExpression('?:', null, IfValue, [ElseValue]);
                                            }
                                            return null;
                                        }
                                        if (!this.CheckSpace(Code)) return null;
                                    }
                                    while (true);
                                }
                            }
                            return null;
                        }
                        return ViewExpression.Null;
                    default: return ViewExpression.Null;
                }
            }
            while (true);
        }
        /**
         * 方法与索引调用解析
         * @param Type 节点类型
         * @param EndCode 结束字符
         */
        private Call(Type: string, EndCode: number): ViewExpression {
            var ParameterIndex = this.Index, Parameter = this.Value();
            if (this.Index - this.Html.length) {
                if (Parameter) {
                    var Parameters = [Parameter];
                    do {
                        var Code = this.Html.charCodeAt(this.Index);
                        if (Code == EndCode) {
                            var Call = new ViewExpression(Type, this.Html.substring(ParameterIndex, this.Index), null, Parameters);
                            return ++this.Index == this.Html.length || Call.SetNext(this.MemberNext(0)) ? Call : null;
                        }
                        //,
                        if (Code == 0x2c) {
                            if (++this.Index == this.Html.length) return null;
                            Parameter = this.Value();
                            if (Parameter && this.Index != this.Html.length) Parameters.push(Parameter);
                            else return null;
                        }
                        else if (!this.CheckSpace(Code)) return null;
                    }
                    while (true);
                }
                if (EndCode == 0x29 && this.Html.charCodeAt(this.Index) == 0x29) {
                    var Call = new ViewExpression(Type, this.Html.substring(ParameterIndex, this.Index), null, []);
                    if (++this.Index == this.Html.length || Call.SetNext(this.MemberNext(0))) return Call;
                }
            }
            return null;
        }
        /**
         * 带符号数字解析
         * @param IsSigned 是否负数
         */
        private Signed(IsSigned: boolean): ViewExpression {
            if (++this.Index != this.Html.length) {
                var Code = this.Html.charCodeAt(this.StartIndex = this.Index);
                if (ViewExpressionBuilder.IsNumber(Code)) return this.Number(Code, IsSigned);
                //.
                if (Code == 0x2e && ++this.Index != this.Html.length && ViewExpressionBuilder.IsNumber(this.Html.charCodeAt(this.Index))) return this.Float(IsSigned);
            }
            return null;
        }
        /**
         * 小数点开头的浮点数解析
         * @param IsSigned 是否负数
         */
        private Float(IsSigned: boolean): ViewExpression {
            do {
                if (++this.Index == this.Html.length) {
                    var Number = parseFloat('0' + this.Html.substring(this.StartIndex, this.Index));
                    return new ViewExpression('0', null, null, null, IsSigned ? -Number : Number);
                }
            }
            while (ViewExpressionBuilder.IsNumber(this.Html.charCodeAt(this.Index)));
            var Number = parseFloat('0' + this.Html.substring(this.StartIndex, this.Index));
            return new ViewExpression('0', null, this.MemberNext(1), null, IsSigned ? -Number : Number).CheckNextNull();
        }
        /**
         * 数字解析
         * @param Code 第一个字符
         * @param IsSigned 是否负数
         */
        private Number(Code: number, IsSigned: boolean): ViewExpression {
            var Number = Code - 0x30;
            if (++this.Index != this.Html.length) {
                if (Number) {
                    do {
                        Code = this.Html.charCodeAt(this.Index);
                        if (!ViewExpressionBuilder.IsNumber(Code)) break;
                        Number = Number * 10 + Code - 0x30;
                        if (++this.Index == this.Html.length) return new ViewExpression('0', null, null, null, IsSigned ? -Number : Number);
                    }
                    while (true);
                    //.
                    if (this.Html.charCodeAt(this.StartIndex = this.Index) == 0x2e) {
                        do {
                            if (++this.Index == this.Html.length) {
                                if (this.Index - this.StartIndex > 1) Number += parseFloat('0' + this.Html.substring(this.StartIndex, this.Index));
                                return new ViewExpression('0', null, null, null, IsSigned ? -Number : Number);
                            }
                        }
                        while (ViewExpressionBuilder.IsNumber(this.Html.charCodeAt(this.Index)));
                        if (this.Index - this.StartIndex > 1) Number += parseFloat('0' + this.Html.substring(this.StartIndex, this.Index));
                    }
                    return new ViewExpression('0', null, this.MemberNext(1), null, IsSigned ? -Number : Number).CheckNextNull();
                }
                if ((this.Html.charCodeAt(this.Index) | 0x20) == 0x78) {
                    if (++this.Index - this.Html.length)
                    {
                        Number = ViewExpressionBuilder.FromHex(this.Html.charCodeAt(this.Index));
                        if (Number >= 0) {
                            do {
                                if (++this.Index == this.Html.length) return new ViewExpression('0', null, null, null, IsSigned ? -Number : Number);
                                Code = ViewExpressionBuilder.FromHex(this.Html.charCodeAt(this.Index));
                                if (Code >= 0) Number = (Number << 4) + Code;
                                else return new ViewExpression('0', null, this.MemberNext(1), null, IsSigned ? -Number : Number).CheckNextNull();
                            }
                            while (true);
                        }
                    }
                    return null;
                }
                return new ViewExpression('0', null, this.MemberNext(1)).CheckNextNull();
            }
            return new ViewExpression('0', null, null, null, IsSigned ? -Number : Number);
        }
        /**
         * 判断字符是否数字
         * @param Code 字符编码
         */
        private static IsNumber(Code: number): boolean  {
            return Code >= 0x30 && Code < 0x3a;
        }
        /**
         * 判断字符是否可以用作成员名称开始
         * @param Code 字符编码
         */
        private static IsMemberNameStart(Code: number): boolean {
            if (Code == 0x5f) return true;//_
            Code |= 0x20;
            return Code >= 0x61 && Code <= 0x7a;
        }
        /**
         * 判断字符是否可以用作成员名称
         * @param Code 字符编码
         */
        private static IsMemberName(Code: number): boolean {
            return this.IsMemberNameStart(Code) || this.IsNumber(Code);
        }
        /**
         * 十六进制字符解析，失败返回 -1
         * @param Code 字符编码
         */
        private static FromHex(Code: number): number {
            if (this.IsNumber(Code)) return Code - 0x30;
            Code |= 0x20;
            if (Code >= 0x61 && Code <= 0x66) return Code - 0x61 + 10;
            return -1;
        }
    }
    class ViewNode {
        /**
         * 数据视图模板
         */
        View: View;
        /**
         * 节点 HMTL 代码
         */
        Html: string;
        /**
         * 节点调用类型
         */
        CallType: string;
        /**
         * 节点表达式
         */
        Expression: ViewExpression;
        /**
         * 节点是否已回合
         */
        IsRound: boolean;
        /**
         * 子节点集合
         */
        Nodes: ViewNode[];
        /**
         * 数据视图模板节点
         * @param View 数据视图模板
         * @param Html 节点 HTML 代码
         * @param CallType 节点调用类型
         * @param Expression 节点表达式
         * @param IsRound 节点是否已回合
         */
        constructor(View: View, Html: string, CallType: string, Expression: ViewExpression, IsRound = false) {
            this.View = View;
            this.Html = Html;
            this.CallType = CallType;
            this.Expression = Expression;
            this.IsRound = IsRound;
        }
    }
    class ViewMark {
        /**
         * 数据视图模板
         */
        View: View;
        /**
         * 数据视图模板节点
         */
        Node: ViewNode;
        /**
         * 标记父节点
         */
        Parent: ViewMark;
        /**
         * 标记标识
         */
        MarkId: number;
        /**
         * 视图数据节点集合，用于表达式查找视图数据父节点
         */
        Datas: ViewData[];
        /**
         * 视图标记集合
         */
        Marks: ViewMark[];
        /**
         * 有效视图标记标识集合
         */
        MarkHash: ViewMark[];
        /**
         * 视图标记
         * @param View 数据视图模板
         * @param Node 数据视图模板节点
         */
        constructor(View: View, Node: ViewNode) {
            this.View = View;
            if (View.Marks) {
                this.MarkId = Pub.GetIdentity();
                this.Parent = View.Marks[View.Marks.length - 1];
                View.Marks.push(this);
                View.MarkIds.push(this.MarkId);
                View.MarkHash[this.MarkId] = this;
                this.Datas = View.Datas.Copy();
                this.Marks = View.Marks.Copy();
                this.MarkHash = [];
                for (var Index = View.MarkIds.length; Index; this.MarkHash.push(View.MarkHash[View.MarkIds[--Index]]));
            }
            else {
                this.MarkId = 0;
                View.Marks = [this];
            }
        }
        /**
         * 视图标记合并
         */
        GetMark(Mark: ViewMark): ViewMark {
            if (this.MarkId) {
                if (!Mark) Mark = this.View.Marks[this.View.Marks.length - 1];
                if (Mark.MarkId) {
                    do {
                        if (this.MarkId == Mark.MarkId) return this;
                        for (var Parent = this.Parent; Parent.MarkId; Parent = Parent.Parent) {
                            if (Parent.MarkId == Mark.MarkId) return Parent;
                        }
                    }
                    while ((Mark = Mark.Parent).MarkId);
                    return this.View.Marks[0];
                }
                return Mark;
            }
            return this;
        }
        /**
         * 设置刷新 HTML 代码的视图标记
         */
        SetRefresh() {
            this.View.SetRefresh(this);
        }
        /**
         * 获取开始标记控件 id
         */
        GetStartId() {
            return '__AutoCSerMarkStart' + this.MarkId + '__';
        }
        /**
         * 获取结束标记控件 id
         */
        GetEndId() {
            return '__AutoCSerMarkEnd' + this.MarkId + '__';
        }
    }
    export class View {
        /**
         * 模板控件 id
         */
        Id: string;
        /**
         * 全局唯一标识
         */
        Identity: number;
        /**
         * 模板根节点
         */
        private Nodes: ViewNode[];
        /**
         * 视图数据节点集合，用于表达式查找视图数据父节点
         */
        Datas: ViewData[];
        /**
         * 当前生成 HTML 代码
         */
        Htmls: string[];
        /**
         * 视图标记集合
         */
        Marks: ViewMark[];
        /**
         * 有效视图标记标识集合
         */
        MarkIds: number[];
        /**
         * 有效视图标记标识集合
         */
        MarkHash: { [key: number]: ViewMark; } = {};
        /**
         * 正在刷新 HTML 代码的视图标记
         */
        private RefreshMark: ViewMark;
        /**
         * 数据视图模板
         * @param Id 模板控件 id
         * @param Html 模板控件 HTML 代码
         */
        constructor(Id: string = null, Html: string = null) {
            this.Id = Id;
            this.Identity = Pub.GetIdentity();
            this.MarkIds = [];
            if (!Html) Html = this.GetElement().Html0().replace(/ @(src|style)=/gi, ' $1=').replace(/select@/gi, 'select');
            var Nodes = [] as ViewNode[], Errors = [], Mark = new ViewMark(this, null);
            for (var Htmls = Html.split('<!--'), Index = 0; Index - Htmls.length;) {
                Html = Htmls[Index];
                if (Index++) {
                    var TypeIndex = Html.indexOf('-->');
                    if (TypeIndex > 1) {
                        for (var Note = Html.substring(0, TypeIndex), IsRound = false, NodeIndex = Nodes.length; NodeIndex;) {
                            var Node = Nodes[--NodeIndex];
                            if (!Node.IsRound) {
                                if (Node.Html == Note) {
                                    Node.IsRound = IsRound = true;
                                    Node.Nodes = Nodes.slice(++NodeIndex, Nodes.length);
                                    Nodes.length = NodeIndex;
                                    Html = Html.substring(TypeIndex + 3);
                                }
                                break;
                            }
                        }
                        if (!IsRound) {
                            var Type = Note, ExpressionIndex = Note.indexOf(':');
                            if (ExpressionIndex > 0) {
                                var NameIndex = Note.indexOf('#');
                                if (NameIndex < 0 || NameIndex > ExpressionIndex) NameIndex = ExpressionIndex;
                                Type = Note.substring(0, NameIndex);
                                switch (Type) {
                                    case 'LOOP':
                                    case 'PUSH':
                                    case 'IF':
                                    case 'NOT':
                                        var ExpressionBuilder = new ViewExpressionBuilder(Note.substring(ExpressionIndex + 1)), Expression = ExpressionBuilder.Value();
                                        if (!Expression) Errors.push(ExpressionBuilder.Html);
                                        Nodes.push(new ViewNode(this, Note, Type, Expression));
                                        Html = Html.substring(TypeIndex + 3);
                                        break;
                                }
                            }
                            else if (ExpressionIndex) {
                                var NameIndex = Note.indexOf('#');
                                if (NameIndex > 0) Type = Note.substring(0, NameIndex);
                                else if (!NameIndex) Type = null;
                                if (Type == 'MARK') {
                                    Nodes.push(new ViewNode(this, Note, Type, null));
                                    Html = Html.substring(TypeIndex + 3);
                                }
                            }
                        }
                    }
                }
                if (Html) {
                    for (var Codes = Html.split('{{'), NodeIndex = 0; NodeIndex - Codes.length;) {
                        Html = Codes[NodeIndex];
                        if (NodeIndex++) {
                            var ExpressionIndex = Html.indexOf('}}');
                            if (ExpressionIndex >= 0) {
                                var ExpressionBuilder = new ViewExpressionBuilder(Html.substring(0, ExpressionIndex)), Expression = ExpressionBuilder.Value();
                                if (!Expression) Errors.push(ExpressionBuilder.Html);
                                Nodes.push(new ViewNode(this, null, 'GET', Expression, true));
                                Html = Html.substring(ExpressionIndex + 2);
                            }
                        }
                        if (Html) Nodes.push(new ViewNode(this, Html, 'HTML', null, true));
                    }
                }
            }
            Htmls = [];
            for (var NodeIndex = Nodes.length; NodeIndex;) {
                var Node = Nodes[--NodeIndex];
                if (!Node.IsRound) {
                    Node.Nodes = Nodes.slice(NodeIndex + 1, Nodes.length);
                    Nodes.length = NodeIndex + 1;
                    Htmls.push(Node.Html);
                }
            }
            this.Nodes = Nodes;
            if (Errors.length || Htmls.length) {
                var Message = '模板 ' + Id + '解析失败：';
                if (Errors.length) Message += '\r\n不可识别表达式 [' + Errors.join('] [') + ']';
                if (Htmls.length) Message += '\r\n标签没有回合 [' + Htmls.join('] [') + ']';
                Pub.SendError({ Message: Message } as ISendError);
            }
        }
        /**
         * 获取模板控件
         */
        GetElement(): HtmlElement {
            return (this.Id ? HtmlElement.$Id(this.Id) : HtmlElement.$(document.body));
        }
        /**
         * 渲染界面
         */
        Show(Data: Object) {
            var Element = this.GetElement().Html(this.ToHtml(Data)).Display(1).GetElements();
            View.OnShowed.Function(Element, Data);
            View.OnSet.Function(Element, Data);
        }
        /**
         * 生成模板代码，不修改模板控件的显示状态（是否渲染界面由模板控件的显示状态决定）
         */
        SetHtml(Data: Object) {
            var Element = this.GetElement().Html(this.ToHtml(Data)).GetElements();
            View.OnSet.Function(Element, Data);
        }
        /**
         * 生成 HTML 代码
         */
        private ToHtml(Data: Object): string {
            this.Datas = [new ViewData(this, Data)];
            this.Htmls = [];
            this.CreateHtml();
            return this.EndHtml();
        }
        /**
         * 生成 HTML 代码最后处理
         */
        EndHtml(): string {
            var Html = this.Htmls.join('').replace(/ src="\{[^"]+"/gi, '').replace(/ @checked="true"/g, ' checked="checked"').replace(/__HTTP__/g, location.protocol);
            this.Datas.length = 1;
            this.Htmls = null;
            return Html;
        }
        /**
         * 生成 HTML 代码
         */
        private CreateHtml() {
            for (var NodeIndex = 0; NodeIndex - this.Nodes.length; this.CreateNode(this.Nodes[NodeIndex++]));
        }
        /**
         * 生成 HTML 代码
         * @param Node 数据视图模板节点
         */
        private CreateNode(Node: ViewNode) {
            if (Node.CallType == 'HTML') this.Htmls.push(Node.Html);
            else if (Node.Expression) {
                var Data = this.GetExpressionData(null, Node.Expression);
                if (Data) {
                    switch (Node.CallType) {
                        case 'GET':
                            if (Data.$Data != null) this.Htmls.push(Data.$Data.toString());
                            break;
                        case 'IF':
                            if (Data.$Data) {
                                for (var NodeIndex = 0; NodeIndex - Node.Nodes.length; this.CreateNode(Node.Nodes[NodeIndex++]));
                            }
                            break;
                        case 'PUSH':
                            if (Data.$Data != null) {
                                this.Datas.push(Data);
                                for (var NodeIndex = 0; NodeIndex - Node.Nodes.length; this.CreateNode(Node.Nodes[NodeIndex++]));
                                this.Datas.pop();
                            }
                            break;
                        case 'LOOP':
                            if (Data.$Data instanceof Array) {
                                for (var Index = 0; Index < Data.$Data.length; ++Index) {
                                    var LoopData = Data.$GetMember(Index);
                                    if (!LoopData) Data[Index] = LoopData = new ViewData(this, Data.$Data[Index]);
                                    this.Datas.push(LoopData);
                                    for (var NodeIndex = 0; NodeIndex - Node.Nodes.length; this.CreateNode(Node.Nodes[NodeIndex++]));
                                    this.Datas.pop();
                                }
                            }
                            break;
                        case 'NOT':
                            if (!Data.$Data) {
                                for (var NodeIndex = 0; NodeIndex - Node.Nodes.length; this.CreateNode(Node.Nodes[NodeIndex++]));
                            }
                            break;
                        case 'MARK':
                            var Mark = new ViewMark(this, Node);
                            this.Htmls.push('<span id="' + Mark.GetStartId() +'" style="display:none"></span>');
                            for (var NodeIndex = 0; NodeIndex - Node.Nodes.length; this.CreateNode(Node.Nodes[NodeIndex++]));
                            this.Htmls.push('<span id="' + Mark.GetEndId() + '" style="display:none"></span>');
                            this.Marks.pop();
                            delete this.MarkHash[this.MarkIds.pop()];
                            break;
                    }
                }
            }
        }
        /**
         * 获取表达式数据
         * @param Data 数据节点
         * @param Expression 节点表达式
         * @param CallThis 方法调用 this 传参
         */
        private GetExpressionData(ViewData: ViewData, Expression: ViewExpression, CallThis = null): ViewData {
            switch (Expression.Type) {
                case 'M':
                    if (ViewData == null) {
                        var Index = this.Datas.length - Expression.Number - 1;
                        if (Index < 0) return this.CheckNextExpressionData(Expression);
                        ViewData = this.Datas[Index];
                    }
                    var Data = CallThis = ViewData.$Data;
                    if (!Data) return this.CheckNextExpressionData(Expression);
                    var NextViewData = ViewData.$GetMember(Expression.Content);
                    if (!NextViewData) {
                        if ((Data = Data[Expression.Content]) === undefined) return this.CheckNextExpressionData(Expression);
                        ViewData[Expression.Content] = NextViewData = new AutoCSer.ViewData(this, Data);
                    }
                    return Expression.Next ? this.GetExpressionData(NextViewData, Expression.Next, CallThis) : NextViewData;
                case '#':
                    NextViewData = (ViewData = this.Datas[0]).$GetMember('Client');
                    if (!NextViewData) ViewData['Client'] = NextViewData = new AutoCSer.ViewData(this, ViewData.$Data.Client);
                    return Expression.Next ? this.GetExpressionData(NextViewData, Expression.Next) : NextViewData;
                case '.':
                case '?.':
                    if (ViewData.$Data == null) {
                        if (Expression.Type.length - 2) return this.CheckNextExpressionData(Expression);
                        return ViewData;
                    }
                    return this.GetExpressionData(ViewData, Expression.Next);
                case '$&':
                    return ViewData.$Data != null ? new AutoCSer.ViewData(this, ViewData.$Data.toString().ToHTML()) : ViewData;
                case '??':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return ViewData.$Data != null || !NextViewData ? ViewData : NextViewData;
                case '(':
                    if (typeof (Data = ViewData.$Data) != 'function') return this.CheckNextExpressionData(Expression);
                    if (!(NextViewData = ViewData.$GetMember(Expression.Content))) {
                        for (var Parameters = [], Index = 0; Index - Expression.Parameters.length; ++Index) {
                            var Parameter = this.GetExpressionData(null, Expression.Parameters[Index]);
                            if (!Parameter) return this.CheckNextExpressionData(Expression);
                            Parameters.push(Parameter.$Data);
                        }
                        if ((Data = (Data as Function).apply(CallThis, Parameters)) === undefined) return this.CheckNextExpressionData(Expression);
                        ViewData[Expression.Content] = NextViewData = new AutoCSer.ViewData(this, Data);
                    }
                    return Expression.Next ? this.GetExpressionData(NextViewData, Expression.Next) : NextViewData;
                case '0':
                    ViewData = new AutoCSer.ViewData(this, Expression.Number);
                    return Expression.Next ? this.GetExpressionData(ViewData, Expression.Next) : ViewData;
                case '"':
                    ViewData = new AutoCSer.ViewData(this, Expression.Content);
                    return Expression.Next ? this.GetExpressionData(ViewData, Expression.Next) : ViewData;
                case 'T':
                    ViewData = new AutoCSer.ViewData(this, true);
                    return Expression.Next ? this.GetExpressionData(ViewData, Expression.Next) : ViewData;
                case 'F':
                    ViewData = new AutoCSer.ViewData(this, false);
                    return Expression.Next ? this.GetExpressionData(ViewData, Expression.Next) : ViewData;
                case '?:':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    var ElseData = this.GetExpressionData(null, Expression.Parameters[0]);
                    return ViewData.$Data ? NextViewData : ElseData;
                case '=':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data == NextViewData.$Data);
                case '!=':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, !NextViewData || ViewData.$Data != NextViewData.$Data);
                case '>':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data > NextViewData.$Data);
                case '>=':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data >= NextViewData.$Data);
                case '<':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data < NextViewData.$Data);
                case '<=':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data <= NextViewData.$Data);
                case '&&':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, NextViewData && ViewData.$Data && NextViewData.$Data);
                case '||':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, ViewData.$Data || (NextViewData && NextViewData.$Data));
                case '!':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    return new AutoCSer.ViewData(this, !NextViewData || !NextViewData.$Data);
                case '+':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data + NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '-':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data - NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '*':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data * NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '/':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data / NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '%':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data % NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '|':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data | NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '&':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data & NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '^':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data ^ NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '>>':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data >> NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '<<':
                    NextViewData = this.GetExpressionData(null, Expression.Next);
                    if (NextViewData) return new AutoCSer.ViewData(this, ViewData.$Data << NextViewData.$Data);
                    return this.CheckNextExpressionData(Expression);
                case '[':
                case '?[':
                    if ((Data = ViewData.$Data) == null) {
                        if (Expression.Type.length - 2) return this.CheckNextExpressionData(Expression);
                        return ViewData;
                    }
                    var NextViewData = ViewData.$GetMember(Expression.Content);
                    if (!NextViewData) {
                        var Parameter = this.GetExpressionData(null, Expression.Parameters[0]);
                        if (!Parameter || (Data = Data[Parameter.$Data]) === undefined) return this.CheckNextExpressionData(Expression);
                        ViewData[Expression.Content] = NextViewData = new AutoCSer.ViewData(this, Data);
                    }
                    return Expression.Next ? this.GetExpressionData(NextViewData, Expression.Next) : NextViewData;
                case ')':
                    ViewData = this.GetExpressionData(null, Expression.Parameters[0]);
                    if (!ViewData) return this.CheckNextExpressionData(Expression);
                    return Expression.Next ? this.GetExpressionData(ViewData, Expression.Next) : ViewData;
            }
        }
        /**
         * 表达式数据获取失败时获取表达式数据
         * @param Expression 节点表达式
         */
        private CheckNextExpressionData(Expression: ViewExpression): ViewData {
            for (Expression = Expression.Next; Expression; Expression = Expression.Next) {
                switch (Expression.Type) {
                    case '??': return this.GetExpressionData(null, Expression.Next);
                    case '?:':
                        this.GetExpressionData(null, Expression.Next);
                        return this.GetExpressionData(null, Expression.Parameters[0]);
                }
            }
        }
        /**
         * 获取视图模板数据节点
         */
        GetData(): ViewData {
            return this.Datas[0];
        }
        /**
         * 设置刷新 HTML 代码的视图标记
         */
        SetRefresh(Mark: ViewMark) {
            if (this.RefreshMark) this.RefreshMark = this.RefreshMark.GetMark(Mark);
            else {
                this.RefreshMark = Mark;
                setTimeout(Pub.ThisFunction(this, this.Refresh), 0);
            }
        }
        /**
         * 刷新 HTML 代码
         */
        private Refresh() {
            var Mark = this.RefreshMark;
            this.RefreshMark = null;
            if (Mark.MarkId) {
                var StartElement = document.getElementById(Mark.GetStartId()), EndMarkId, EndElement, ParentElement;
                if (StartElement && (EndElement = document.getElementById(EndMarkId = Mark.GetEndId()))) ParentElement = HtmlElement.$ParentElement(StartElement);
                if (!ParentElement || ParentElement != HtmlElement.$ParentElement(EndElement)) {
                    if (this.Id && !document.getElementById(this.Id)) return;
                    Mark = this.Marks[0];
                }
                if (Mark.MarkId) {
                    this.Datas = Mark.Datas.Copy();
                    this.Marks = Mark.Marks.Copy();
                    this.MarkIds = [];
                    this.MarkHash = {};
                    for (var Index = Mark.MarkHash.length; Index;) {
                        var HashMark = Mark.MarkHash[--Index];
                        this.MarkIds.push(HashMark.MarkId);
                        this.MarkHash[HashMark.MarkId] = HashMark;
                    }
                    this.Htmls = [];
                    for (var Index = 0, Nodes = Mark.Node.Nodes; Index - Nodes.length; this.CreateNode(Nodes[Index++]));
                    for (var Element = HtmlElement.$NextElement(StartElement); Element && Element.id != EndMarkId; Element = HtmlElement.$NextElement(StartElement)) HtmlElement.$Delete(Element, ParentElement);
                    Pub.DeleteElements.Html(this.EndHtml()).Child().InsertBefore(EndElement, ParentElement);
                    View.OnSet.Function(Element);
                    return;
                }
            }
            this.SetHtml(this.GetData().$Data);
        }
        /**
         * 设置不显示 style.dispaly = none
         */
        Hide() {
            this.GetElement().Display(0);
        }
        /**
         * 删除标记控件
         */
        static DeleteMark(Span: HTMLElement) {
            var Id = Span.id;
            if (Id && Id.length > 14 && Id.substring(0, 14) == '__AutoCSerMark') HtmlElement.$Delete(Span);
        }
        /**
         * 数据视图渲染整体界面以后触发的操作事件
         */
        static OnShowed: Events = new Events();
        /**
         * 数据视图重置数据后触发的操作事件
         */
        static OnSet: Events = new Events();
        /**
         * 数据视图模板集合，关键字为控件 id
         */
        static Views: { [key: string]: View; } = {};
        /**
         * 页面模板 document.body
         */
        static Body: View;
        /**
         * 头部模板 document.title
         */
        private static Header: View;
        /**
         * 更新 document.title
         */
        static ChangeHeader() {
            document.title = this.Header.ToHtml(this.Body.GetData().$Data);
        }
        /**
         * 数据视图模板初始化
         */
        static Create(PageView: PageView) {
            for (var Childs = HtmlElement.$('@view').GetElements(), Index = Childs.length; Index; this.Views[Id] = new View(Id)) {
                var Child = Childs[--Index], Id = Child.id;
                if (!Id) Child.id = Id = HtmlElement.$Attribute(Child, 'view');
            }
            if (PageView.IsLoadView) {
                this.Header = new View(null, document.title);
                var ViewOver = document.getElementById('__VIEWOVERID__');
                if (ViewOver) {
                    var Display = ViewOver.style.display;
                    ViewOver.style.display = 'none';
                    //this.Body = new View(null, null, PageView.OnShowed, PageView.OnSet);
                    this.Body = new View(null, null);
                    ViewOver.style.display = Display || '';
                }
                else this.Body = new View(null, null);
                //else this.Body = new View(null, null, PageView.OnShowed, PageView.OnSet);
            }
        }
    }
    class PageView {
        /**
         * 视图数据是否加载完毕
         */
        IsLoad: boolean;
        /**
         * 是否需要加载数据视图
         */
        IsLoadView: boolean;
        /**
         * 视图数据是否加载完毕
         */
        IsView: boolean;
        /**
         * 视图数据是否加载错误
         */
        LoadError: boolean;
        /**
         * 返回值状态，1 表示成功
         */
        State: number;
        /**
         * 附加错误信息
         */
        Message: string;
        /**
         * 重定向地址
         */
        Location: string;
        /**
         * 接口返回数据结果
         */
        Result: any;
    }
    /*include:loadSendError*/
    export class BrowserEvent {
        /**
         * 原浏览器事件
         */
        Event: Event;
        /**
         * 触发事件的控件 target / srcElement
         */
        Element: HTMLElement;
        /**
         * 鼠标事件位置横坐标 pageX / clientX
         */
        MouseX: number;
        /**
         * 鼠标事件位置纵坐标 pageY / clientY
         */
        MouseY: number;
        /**
         * 键盘事件键值 which / keyCode
         */
        KeyCode: number;
        /**
         * 事件是否成功，没有被取消
         */
        Return: boolean;
        /**
         * 兼容 IE 浏览器事件
         * @param Event 浏览器事件
         */
        constructor(Event: any) {
            if (this.Event = Event as Event || event) {
                this.Element = (Pub.IE ? this.Event.srcElement : this.Event.target) as HTMLElement;
                this.MouseX = Pub.IE ? (this.Event as MouseEvent).clientX : (this.Event as MouseEvent).pageX;
                this.MouseY = Pub.IE ? (this.Event as MouseEvent).clientY : (this.Event as MouseEvent).pageY;
                this.KeyCode = Pub.IE ? (this.Event as KeyboardEvent).keyCode : (this.Event as KeyboardEvent).which;
            }
            this.Return = true;
        }
        /**
         * 取消冒泡操作
         */
        CancelBubble() {
            this.Return = false;
            if (Pub.IE) this.Event.returnValue = false;
            else this.Event.preventDefault();
        }
        /**
         * 根据属性名称查找控件，如果当前控件属性不匹配则继续匹配父节点控件直到顶层节点（不查找子节点）
         * @param AttributeName 匹配属性名称
         * @param Value 默认为 null 表示仅匹配属性名称，否则需要匹配属性值
         */
        $ParentName(AttributeName: string, Value: string = null): HTMLElement {
            return HtmlElement.$ParentName(this.Element, AttributeName, Value);
        }
    }
    export class DeclareEvent extends BrowserEvent {
        /**
         * 声明组件控件 id
         */
        DeclareId: string;
        /**
         * 默认为 false 表示需要触发初始化组件操作，设置为 true 表示仅获取组件定义不做初始化操作处理
         */
        IsGetOnly: boolean;
        /**
         * 声明组件事件
         * @param Id 控件 id
         * @param IsGetOnly 默认为 true 表示仅获取组件定义不做初始化操作处理，设置为 false 表示需要触发初始化组件操作，在组件 Start 初始化操作中应该先判断该属性为 false 才触发初始化组件操作
         */
        constructor(Id: string, IsGetOnly = true) {
            var Element = HtmlElement.$IdElement(Id);
            super({ srcElement: Element, target: Element });
            this.DeclareId = Id;
            this.IsGetOnly = IsGetOnly;
        }
    }
    export interface IDeclareParameter {
        /**
         * 声明组件控件 id
         */
        Id: string;
        /**
         * 声明组件事件
         */
        Event: DeclareEvent;
        /**
         * 声明组件控件
         */
        DeclareElement: HTMLElement;
    }
    export interface IDeclare {
        /**
         * 声明组件初始化调用
         */
        Start(Event: DeclareEvent);
    }
    export interface IDeclareReset  {
        /**
         * 支持重置初始化的声明组件重置调用
         */
        Reset(Parameter: IDeclareParameter, Element: HTMLElement);
    }
    export enum DeclareType {
        /**
         * 事件触发控件（适合 input 控件），属性名称为组件定义名称小写字母，该属性值为构造函数调用参数 JSON 字符串
         */
        EventElement = 'EventElement',
        /**
         * 通过 $ParentName 查找第一个属性名称与组件定义名称小写字母相同的控件，该属性值为构造函数调用参数 JSON 字符串
         */
        ParentName = 'ParentName',
        /**
         * 通过 $ParentName 查找第一个属性名称与组件定义名称小写字母相同的控件，如果该属性值 JSON 字符串解析出 Id 属性对象，则根据该 Id 值重定向控件，一般用于两个相关联的控件
         */
        ParameterId = 'ParameterId'
    }
    export enum DeclareManyType {
        /**
         * 通过 $ParentName 查找第一个属性名称与组件定义名称小写字母相同的控件，该属性值为构造函数调用参数 JSON 字符串
         */
        ResetParentName = 'ResetParentName'
    }
    export class Declare {
        /**
         * 组件类型（组件函数名称）
         */
        private Type: string;
        /**
         * document.body 绑定事件名称
         */
        private EventName: string;
        /**
         * 组件名称
         */
        private Name: string;
        /**
         * 组件模板名称（小写字母），用于控件查找操作，模板名称必须为小写字母
         */
        private LowerName: string;
        /**
         * 组件实例对象集合名称 Name + 's'
         */
        private AutoCSerName: string;
        /**
         * 组件声明
         * @param Function 组件构造函数，必须继承 IDeclare 或者 IDeclareMany
         * @param Name 组件名称，对应模板名称必须为小写字母
         * @param EventName document.body 绑定事件名称
         * @param Type 组件类型，IDeclare 支持 Src / AttributeName / ParameterId，IDeclareMany 支持 ParameterMany
         */
        constructor(Function: Function, Name: string, EventName: string, Type: string) {
            this.Type = Type;
            this.EventName = EventName;
            this.LowerName = (this.Name = Name).toLowerCase();
            this.AutoCSerName = Name + 's';
            Pub.Functions[Name] = Function;
            Declare.Getters[this.Name] = Pub.ThisFunction(this, this.Get) as (Id: string, IsGetOnly: boolean) => any;
            Pub.OnLoad(this.Load, this, true);
        }
        /**
         * 声明组件初始化
         */
        private Load() {
            Declare.Declares[this.AutoCSerName] = {};
            HtmlElement.$(document.body).AddEvent(this.EventName, Pub.ThisEvent(this, this[this.Type]));
        }
        /**
         * 创建声明组件实例
         */
        private NewDeclare(Parameter: IDeclareParameter): IDeclare {
            return new (Pub.Functions[this.Name] as new (Parameter: IDeclareParameter) => IDeclare)(Parameter);
        }
        private EventElement(Event: DeclareEvent): IDeclare {
            var Element = Event.Element, ParameterString = HtmlElement.$Attribute(Element, this.LowerName);
            if (ParameterString != null) {
                var Id = Element.id;
                if (Id) {
                    var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
                    if (Value) Declare.TryStart(Value, Event);
                    else Values[Id] = Value = this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element, Id));
                    return Value;
                }
                return this.NewDeclare(Declare.GetParameter(ParameterString, Event, Element));
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private ParentName(Event: DeclareEvent): IDeclare {
            var Element = Event.$ParentName(this.LowerName);
            if (Element) {
                var Id = Element.id;
                if (Id) {
                    var Values = Declare.Declares[this.AutoCSerName], Value = Values[Id];
                    if (Value) Declare.TryStart(Value, Event);
                    else Values[Id] = Value = this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element, Id));
                    return Value;
                }
                return this.NewDeclare(Declare.GetParameter(HtmlElement.$Attribute(Element, this.LowerName), Event, Element));
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private ParameterId(Event: DeclareEvent): IDeclare {
            var Element = Event.$ParentName(this.LowerName);
            if (Element) {
                var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                if (Parameter.Id) {
                    Element = HtmlElement.$IdElement(Parameter.Id);
                    if (!Element) return Declare.Declares[this.AutoCSerName][Parameter.Id];
                    Parameter = null;
                }
                var Id = Element.id, Values = Declare.Declares[this.AutoCSerName], Value;
                if (Id) Value = Values[Id];
                else Element.id = Id = Declare.NextId();
                if (Value) Value.Start(Event);
                else {
                    if (Parameter == null) Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                    Parameter.Event = Event;
                    Parameter.DeclareElement = Element;
                    Values[Parameter.Id = Id] = Value = this.NewDeclare(Parameter);
                }
                return Value;
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId];
        }
        private ResetParentName(Event: DeclareEvent): IDeclareReset {
            if (!Event.IsGetOnly) {
                var Element = Event.$ParentName(this.LowerName);
                if (Element) {
                    var Parameter = eval('(' + HtmlElement.$Attribute(Element, this.LowerName) + ')') as IDeclareParameter;
                    Parameter.DeclareElement = Element;
                    Parameter.Event = Event;
                    if (Parameter.Id) {
                        var Value = Declare.Declares[this.AutoCSerName][Parameter.Id] as Object as IDeclareReset;
                        if (Value) {
                            Value.Reset(Parameter, Element);
                            return Value;
                        }
                        Declare.Declares[this.AutoCSerName][Parameter.Id] = (Value = this.NewDeclare(Parameter) as Object as IDeclareReset) as Object as IDeclare;
                        return Value;
                    }
                }
            }
            return Declare.Declares[this.AutoCSerName][Event.DeclareId] as Object as IDeclareReset;
        }
        /**
         * 获取声明组件实例
         * @param Id 控件 id
         * @param IsGetOnly 默认为 true 表示仅获取组件定义不做初始化操作处理，设置为 false 表示需要触发初始化组件操作，在组件 Start 初始化操作中应该先判断该属性为 false 才触发初始化组件操作
         */
        private Get(Id: string, IsGetOnly = true) {
            return this[this.Type](new DeclareEvent(Id, IsGetOnly));
        }
        /**
         * 生成声明组件控件 id
         */
        private static NextId(): string {
            return '_' + Pub.GetIdentity() + '_DECLARE_';
        }
        /**
         * 组件控件参数解析
         */
        private static GetParameter(ParameterString: string, Event: DeclareEvent, Element: HTMLElement, Id: string = null): IDeclareParameter {
            var Parameter = (ParameterString ? eval('(' + ParameterString + ')') : {}) as IDeclareParameter;
            Parameter.Id = Id || Declare.NextId();
            Parameter.DeclareElement = Element;
            Parameter.Event = Event;
            return Parameter;
        }
        /**
         * 创建组件声明
         * @param Function 组件构造函数，必须继承 IDeclare
         * @param Name 组件名称，对应模板名称必须为小写字母
         * @param EventName document.body 绑定事件名称，移除 on 前缀，比如 onclick 传参 click
         * @param Type 组件类型
         */
        static Create(Function: new (Parameter: any) => IDeclare, Name: string, EventName: string, Type: DeclareType): Declare {
            return new Declare(Function as Object as Function, Name, EventName, Type);
        }
        /**
         * 创建支持重置初始化的组件声明
         * @param Function 组件构造函数，必须继承 IDeclareReset
         * @param Name 组件名称，对应模板名称必须为小写字母
         * @param EventName document.body 绑定事件名称，移除 on 前缀，比如 onclick 传参 click
         * @param Type 组件类型
         */
        static CreateMany(Function: new (Parameter: any) => IDeclareReset, Name: string, EventName: string, Type: DeclareManyType): Declare {
            return new Declare(Function as Object as Function, Name, EventName, Type);
        }
        /**
         * 尝试调用声明组件初始化操作
         */
        static TryStart(Value: IDeclare, Event: DeclareEvent) {
            if (Event && !Event.IsGetOnly) Pub.ThisFunction(Value, Value.Start)(Event);
        }
        /**
         * 获取声明组件实例对象函数
         * @param Id 组件标识
         * @param IsGetOnly 默认为 true 表示仅获取组件定义不做初始化操作处理，设置为 false 表示需要触发初始化组件操作
         */
        static Getters: { [key: string]: (Id: string, IsGetOnly: boolean) => IDeclare } = {};
        /**
         * 声明组件实例对象集合，关键字为 组件名称+'s'
         */
        static Declares: { [key: string]: { [key: string]: IDeclare } } = {};
    }
    export interface ICookieValue {
        Name: string;
        Value: any;
        Expires: Date;
        Path: string;
        Domain: string;
        /**
         * HTTPS 参数
         */
        Secure: boolean;
    }
    export class Cookie {
        private static DefaultParameter = { Expires: null, Path: '/', Domain: location.hostname, Secure: null };
        /**
         * 超时毫秒数
         */
        private Expires: number;
        /**
         * 生效路径
         */
        private Path: string;
        /**
         * 生效域名
         */
        private Domain: string;
        /**
         * HTTPS 参数
         */
        private Secure: boolean;
        /**
         * 初始化复制参数，复制属性名称可以为 Expires / Path / Domain / Secure
         */
        constructor(Parameter) {
            Pub.GetParameter(this, Cookie.DefaultParameter, Parameter);
        }
        /**
         * 写入 cookie，参考 ICookieValue 定义
         */
        Write(Value: any) {
            this.WriteCookie(Value as ICookieValue);
        }
        /**
         * 写入 cookie
         */
        private WriteCookie(Value: ICookieValue) {
            var Cookie = encodeURI(Value.Name) + '=' + (Value.Value == null ? '.' : encodeURI(Value.Value.toString())), Expires = Value.Expires;
            if (Value.Value == null) Expires = new Date;
            else if (!Expires && this.Expires) Expires = (new Date).AddMilliseconds(this.Expires);
            if (Expires) Cookie += '; expires=' + Expires['toGMTString']();
            var Path = Value.Path || this.Path;
            if (Path) Cookie += '; path=' + Path;
            var Domain = Value.Domain || this.Domain;
            if (Domain) Cookie += '; domain=' + Domain;
            var Secure = Value.Secure || this.Secure;
            if (Secure) Cookie += '; secure=' + Secure.toString();
            document.cookie = Cookie;
        }
        /**
         * 读取 cookie 值
         */
        Read(Name: string, DefaultValue: string = null): string {
            for (var Values = document.cookie.split('; '), Value = null, Index = Values.length; --Index >= 0 && Value == null;) {
                var IndexOf = Values[Index].indexOf('=');
                if (decodeURI(Values[Index].substring(0, IndexOf)) == Name) Value = decodeURI(Values[Index].substring(IndexOf + 1));
            }
            return Value || DefaultValue;
        }
        /**
         * 读取 cookie 值并解析为 JSON 对象
         */
        ReadJson(Name: string, DefaultValue: any): any {
            var Value = this.Read(Name, null);
            return Value ? eval('(' + Value + ')') : DefaultValue;
        }
        /**
         * 默认 cookie 操作
         */
        static Default: Cookie = new Cookie({ Expires: 24 * 3600 * 1000 });
    }
}
setTimeout(AutoCSer.Pub.LoadIE, 0, 'javascript');