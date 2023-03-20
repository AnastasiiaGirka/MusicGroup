import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ContactsComponent} from "./components/contacts/contacts.component";
import {HomeComponent} from "./components/home/home.component";
import {AlbumComponent} from "./components/album/album.component";

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'albums', component: AlbumComponent },
  { path: 'contacts', component: ContactsComponent },
  // Add more routes as needed
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
