/// <reference path = "./load.page.ts" />
'use strict';
module AutoCSer {
    export class Menu {
        private static NenuParameter = { Id: null, Event: null, MenuId: null, ViewData: null, ShowViewFunctionName: null, Type: null, Top: null, Left: null, OverClassName: null, OutClassName: null, IsCheckScroll: 1, IsShow: 1, IsMove: 1, ZIndex: 1 };
        private static MenuEvents = { OnStart: null, OnShowed: null };
        Id: string;
        MenuId: string;
        Event: DeclareEvent;
        Type: string;
        ViewData: any;
        ShowViewFunctionName: string;
        IsMove: boolean;
        private Top: number;
        private Left: number;
        private IsCheckScroll: boolean;
        private ZIndex: number;
        private OverClassName: string;
        private OutClassName: string;
        IsShow: boolean;

        OnStart: Events;
        OnShowed: Events;

        Element: HTMLElement;
        ShowView: Object;
        IsOver: boolean;
        constructor(Parameter) {
            Pub.GetParameterEvents(this, Menu.NenuParameter, Menu.MenuEvents, Parameter);
        }

        ShowMenu() {
            var Menu = HtmlElement.$Id(this.MenuId), View;
            if (this.IsMove) Menu.Style('position', 'absolute').Style('zIndex', HtmlElement.ZIndex + this.ZIndex);
            if (this.ViewData) View = AutoCSer.View.Views[this.MenuId];
            var IsShow = this.ShowViewFunctionName == null || this.ViewData[this.ShowViewFunctionName]();
            if (View && this.ShowView != this.ViewData) {
                this.ShowView = this.ViewData;
                if (IsShow) View.Show(this.ViewData);
            }
            if (this.IsShow && IsShow) Menu.Display(1);
            var Element = HtmlElement.$(this.Element);
            if (this.OutClassName) Element.RemoveClass(this.OutClassName);
            if (this.OverClassName) Element.AddClass(this.OverClassName);
        }
        HideMenu() {
            var Element = HtmlElement.$(this.Element);
            if (this.OverClassName) Element.RemoveClass(this.OverClassName);
            if (this.OutClassName) Element.AddClass(this.OutClassName);
            if (this.IsShow) HtmlElement.$Id(this.MenuId).Display(0);
            this.ShowView = null;
        }
        CheckScroll(Left: number, Top: number, XY: IPointer = null) {
            if (this.Left) Left += this.Left;
            if (this.Top) Top += this.Top;
            var Menu = HtmlElement.$Id(this.MenuId);
            if (this.IsCheckScroll) {
                var ScrollLeft = HtmlElement.$GetScrollLeft(), ScrollTop = HtmlElement.$GetScrollTop();
                if (XY) {
                    var Width = HtmlElement.$Width(Menu.Element0()), Height = HtmlElement.$Height(Menu.Element0());
                    if (Width) {
                        var ClientWidth = HtmlElement.$Width() + ScrollLeft;
                        if (ClientWidth > XY.Left && Left > (ClientWidth -= Width)) Left = ClientWidth;
                    }
                    if (Height) {
                        var ClientHeight = HtmlElement.$Height() + ScrollTop;
                        if (ClientHeight > XY.Top && Top > (ClientHeight -= Height)) Top = ClientHeight;
                    }
                }
                if (Left < ScrollLeft) Left = ScrollLeft;
                if (Top < ScrollTop) Top = ScrollTop;
            }
            Menu.ToXY(Left, Top);
        }
        private ToTopLeft(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left, XY.Top - HtmlElement.$Height(HtmlElement.$IdElement(this.MenuId)), XY);
        }
        private ToTopRight(Event: BrowserEvent, Element: HtmlElement) {
            var Menu = HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left + HtmlElement.$Width(Element.Element0()) - HtmlElement.$Width(Menu.Element0()), XY.Top - HtmlElement.$Height(Menu.Element0()), XY);
        }
        ToTop(Event: BrowserEvent, Element: HtmlElement) {
            var Menu = HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left + (HtmlElement.$Width(Element.Element0()) - HtmlElement.$Width(Menu.Element0())) / 2, XY.Top - HtmlElement.$Height(Menu.Element0()), XY);
        }
        private ToBottomLeft(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left, XY.Top + HtmlElement.$Height(Element.Element0()), XY);
        }
        private ToBottomRight(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + HtmlElement.$Width(Element.Element0()) - HtmlElement.$Width(HtmlElement.$IdElement(this.MenuId)), XY.Top + HtmlElement.$Height(Element.Element0()), XY);
        }
        private ToBottom(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + (HtmlElement.$Width(Element.Element0()) - HtmlElement.$Width(HtmlElement.$IdElement(this.MenuId))) / 2, XY.Top + HtmlElement.$Height(Element.Element0()), XY);
        }
        private ToLeftTop(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left - HtmlElement.$Width(HtmlElement.$IdElement(this.MenuId)), XY.Top, XY);
        }
        private ToLeftBottom(Event: BrowserEvent, Element: HtmlElement) {
            var Menu = HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left - HtmlElement.$Width(Menu.Element0()), XY.Top + HtmlElement.$Height(Element.Element0()) - HtmlElement.$Height(Menu.Element0()), XY);
        }
        private ToLeft(Event: BrowserEvent, Element: HtmlElement) {
            var Menu = HtmlElement.$Id(this.MenuId), XY = Element.XY0();
            this.CheckScroll(XY.Left - HtmlElement.$Width(Menu.Element0()), XY.Top + (HtmlElement.$Height(Element.Element0()) - HtmlElement.$Height(Menu.Element0())) / 2, XY);
        }
        private ToRightTop(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + HtmlElement.$Width(Element.Element0()), XY.Top, XY);
        }
        private ToRightBottom(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + HtmlElement.$Width(Element.Element0()), XY.Top + HtmlElement.$Height(Element.Element0()) - HtmlElement.$Height(HtmlElement.$IdElement(this.MenuId)), XY);
        }
        private ToRight(Event: BrowserEvent, Element: HtmlElement) {
            var XY = Element.XY0();
            this.CheckScroll(XY.Left + HtmlElement.$Width(Element.Element0()), XY.Top + (HtmlElement.$Height(Element.Element0()) - HtmlElement.$Height(HtmlElement.$IdElement(this.MenuId))) / 2, XY);
        }
    }
}