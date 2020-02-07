import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { PostModel } from '../components/post-home-items/post-home-items.component';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PostsService {

  constructor(private http: HttpClient){}

  getAll(): Observable<any>{
    return this.http.get('https://localhost:44343/api/Posts');
  }

}
