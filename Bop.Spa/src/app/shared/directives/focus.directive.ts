import { Directive, ElementRef, OnInit, Renderer2 } from "@angular/core";

@Directive({ selector: '[tmFocus]' })

export class myFocus implements OnInit {
    constructor(private el: ElementRef, private renderer: Renderer2) {
        // focus won't work at construction time - too early
    }

    ngOnInit() {
        this.el.nativeElement.focus();
    }
}