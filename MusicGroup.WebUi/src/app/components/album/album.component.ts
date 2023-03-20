import {Component, Input, OnInit} from '@angular/core';
import {AlbumService} from "../../services/album.service";
import {Album} from "../../models/album";

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {

  private _albumService;

  albums: Album[] = [];
  albumForm: boolean = false;
  isNewAlbum: boolean = false;
  newAlbum: Album | null = new Album();
  editAlbumForm: boolean = false;
  editedAlbum: Album | null = null;

  constructor(private albumService: AlbumService) {
    this._albumService = albumService;
  }

  ngOnInit(): void {
    this.list();
  }

  list(): void {
    this._albumService.listAlbums().subscribe(
      result => {
        this.albums = result;
      }
    );
  }

  saveAlbum() {
    const album: Album | null = (this.isNewAlbum)
      ? this.newAlbum
      : (this.editedAlbum != null)
        ? this.editedAlbum
        : null;

    if (album) {
      this._albumService.saveAlbum(album.id, album.name).subscribe(
        result => {
          if (this.isNewAlbum) {
            this.albums.push(result);
          } else {
            // const position: number = this.albums.indexOf(result);
            // this.albums[position] = result;
          }
        }
      );
    }

    this.resetForms();
  }

  removeAlbum(album: Album) {
    this._albumService.removeAlbum(album.id);
  }

  resetForms() {
    this.editedAlbum = null;
    this.newAlbum = null;
    this.editAlbumForm = false;
    this.albumForm = false;
  }

  showEditAlbumForm(album: Album) {
    if (!album) {
      this.albumForm = false;
      return;
    }
    this.editAlbumForm = true;
    this.isNewAlbum = false;
    this.editedAlbum = album;
  }

  showAddAlbumForm() {
    this.newAlbum = new Album();
    this.albumForm = true;
    this.isNewAlbum = true;
  }
}
