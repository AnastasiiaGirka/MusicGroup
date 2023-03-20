import { Component } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})

export class MenuComponent {
  menuItems = [
    { label: 'Home', link: '/' },
    { label: 'Albums', link: '/albums' },
    { label: 'Contacts', link: '/contacts' }
  ];
}
