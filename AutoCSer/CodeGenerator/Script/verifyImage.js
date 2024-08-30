//验证图片	<img verifyimage="{Width:80,Height:20,ButtonId:'YYY'}" id="XXX" />
var AutoCSer;
(function (AutoCSer) {
    var VerifyImage = /** @class */ (function () {
        function VerifyImage(Parameter) {
            AutoCSer.Pub.GetParameterEvents(this, VerifyImage.DefaultParameter, VerifyImage.DefaultEvents, Parameter);
            AutoCSer.Declare.TryStart(this, this.Event);
        }
        VerifyImage.prototype.Start = function (Event) {
            var Element = AutoCSer.HtmlElement.$Id(this.Id), Image = Element.Element0();
            if (Image != this.Element) {
                this.Element = Image;
                Element.Set('alt', '验证码').Set('border', 0);
                if (this.Width)
                    Element.Set('width', this.Width);
                if (this.Height)
                    Element.Set('height', this.Height);
                AutoCSer.HtmlElement.$Id(this.ButtonId).Cursor('pointer').AddEvent('click', AutoCSer.Pub.ThisFunction(this, this.ClickButton));
                this.Show(false);
            }
        };
        VerifyImage.prototype.ClickButton = function () {
            this.Show(true);
            this.OnClick.Function();
        };
        VerifyImage.prototype.Show = function (IsRefresh) {
            var Verify = IsRefresh ? null : AutoCSer.Cookie.Default.Read('VerifyImage');
            if (!Verify)
                Verify = (new Date).getTime();
            AutoCSer.Cookie.Default.Write({ Name: 'VerifyImage', Value: Verify, Expires: (new Date).AddMinutes(20) });
            AutoCSer.HtmlElement.$Id(this.Id).Set('src', '/verifyImage?t=' + Verify).Display(1);
        };
        VerifyImage.prototype.Clear = function () {
            AutoCSer.Cookie.Default.Write({ Name: 'VerifyImage' });
        };
        VerifyImage.DefaultParameter = { Id: null, Event: null, Width: null, Height: null, ButtonId: null };
        VerifyImage.DefaultEvents = { OnClick: null };
        return VerifyImage;
    }());
    AutoCSer.VerifyImage = VerifyImage;
    AutoCSer.Declare.Create(VerifyImage, 'VerifyImage', 'mouseover', AutoCSer.DeclareType.ParentName);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=verifyImage.js.map