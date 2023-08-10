import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[noCopy]'
})
export class NoCopyDirective {
  @HostListener('copy', ['$event'])
  onCopy(event: ClipboardEvent) {
    // Clear the copied data
    event.clipboardData?.setData('text/plain', '');
    event.preventDefault();
  }
}