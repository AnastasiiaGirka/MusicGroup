import {Song} from "./song";

export class Album {
  id?: number |  null = null;

  name: string= "";

  songs: Song[] = [];
}
