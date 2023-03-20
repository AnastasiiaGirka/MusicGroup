import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Album} from "../models/album";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";
import {SaveAlbumRequest} from "../models/requests/saveAlbumRequest";

@Injectable({
  providedIn: 'root'
})
export class AlbumService {

  constructor(private http: HttpClient) { }

  public getAlbum(id: number): Observable<Album>{
    return this.http.get<Album>(`${environment.apiUrl}/GetAlbum`, { params: {id} });
  }
  public saveAlbum(name: string): Observable<Album>{
    const request = new SaveAlbumRequest();
    request.name = name;

    return this.http.post<Album>(`${environment.apiUrl}/SaveAlbum`, request);
  }
  public listAlbums(): Observable<Album[]>{
    return this.http.get<Album[]>(`${environment.apiUrl}/ListAlbums`);
  }
}
