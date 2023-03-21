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

  public deleteAlbum(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiUrl}/Album/DeleteAlbum`, { params: {id} });
  }

  public getAlbum(id: string): Observable<Album>{
    return this.http.get<Album>(`${environment.apiUrl}/Album/GetAlbum`, { params: {id} });
  }
  public saveAlbum(id: string, name: string): Observable<Album>{
    const request = new SaveAlbumRequest();
    request.id = id;
    request.name = name;

    return this.http.post<Album>(`${environment.apiUrl}/Album/SaveAlbum`, request);
  }
  public listAlbums(): Observable<Album[]>{
    return this.http.get<Album[]>(`${environment.apiUrl}/Album/ListAlbums`);
  }
}
