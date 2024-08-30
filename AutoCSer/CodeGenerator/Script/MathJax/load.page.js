/// <reference path = "../load.page.ts" />
'use strict';
/*include:MathJax*/
var AutoCSer;
(function (AutoCSer) {
    var MathJaxLoader = /** @class */ (function () {
        function MathJaxLoader() {
        }
        MathJaxLoader.Start = function () {
            var Config = document.createElement('script');
            Config.type = 'text/x-mathjax-config';
            Config.text = 'AutoCSer.MathJaxLoader.LoadConfig();';
            document.getElementsByTagName('head')[0].appendChild(Config);
            AutoCSer.View.OnSet.Add(AutoCSer.Pub.ThisFunction(this, this.Show));
            AutoCSer.Pub.LoadModule('MathJax/load');
            this.Show();
        };
        MathJaxLoader.Get = function () {
            return window['MathJax'];
        };
        MathJaxLoader.LoadConfig = function () {
            this.Get().Hub.Config({ extensions: ['tex2jax.js'], jax: ['input/TeX', 'output/HTML-CSS'], elements: [''], tex2jax: { inlineMath: [['$', '$'], ['\\(', '\\)']] } });
        };
        MathJaxLoader.CheckShowElement = function (Element) {
            return !AutoCSer.HtmlElement.$ParentName(Element, 'htmleditor');
        };
        MathJaxLoader.Show = function () {
            for (var MathJax = this.Get(), Values = [], Elements = AutoCSer.HtmlElement.$('@lang=latex').GetElements(), Index = 0; Index - Elements.length; ++Index) {
                if (MathJaxLoader.CheckShowElement(Elements[Index])) {
                    var Element = Elements[Index], Nodes = Element.childNodes;
                    if (Nodes.length) {
                        var Span = Nodes[0];
                        if (Span.tagName && Span.tagName.toLowerCase() == 'span') {
                            var Text = AutoCSer.HtmlElement.$GetText(Span);
                            if (Text) {
                                Element.innerHTML = this.Format(Text);
                                Values.push(Element);
                            }
                        }
                    }
                }
            }
            if (Values.length)
                MathJax.Hub.Queue(['Typeset', MathJax.Hub, Values, {}], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [null, Values]));
        };
        MathJaxLoader.FixedBorder = function (Element, Elements) {
            if (Elements === void 0) { Elements = null; }
            if (Elements) {
                for (var Index = 0; Index - Elements.length; this.FixedBorder(Elements[Index++]))
                    ;
            }
            else if (Element) {
                for (var Elements = new AutoCSer.HtmlElement('.MathJax/nobr/span', Element).GetElements(), Index = Elements.length; Index;) {
                    var Nodes = Elements[--Index].childNodes;
                    if (Nodes.length > 1) {
                        var Span = Nodes[1];
                        if (Span.style.borderLeftWidth == '0.003em' || Span.style.borderLeftWidth == '0.002em')
                            Span.style.borderLeftWidth = '0';
                    }
                }
            }
        };
        MathJaxLoader.Format = function (Text) {
            return '$\n' + Text.replace(/\xA0/g, ' ').ToHTML() + '\n$';
        };
        MathJaxLoader.ShowElement = function (Element) {
            var MathJax = this.Get();
            MathJax.Hub.Queue(['Typeset', MathJax.Hub, Element], AutoCSer.Pub.ThisFunction(MathJaxLoader, MathJaxLoader.FixedBorder, [Element]));
        };
        MathJaxLoader.TryShow = function (Element, Text) {
            if (Text) {
                Element.innerHTML = this.Format(Text);
                this.ShowElement(Element);
            }
            else
                Element.innerHTML = '';
        };
        return MathJaxLoader;
    }());
    AutoCSer.MathJaxLoader = MathJaxLoader;
    AutoCSer.Pub.OnLoad(MathJaxLoader.Start, MathJaxLoader, true);
})(AutoCSer || (AutoCSer = {}));
//# sourceMappingURL=load.page.js.map