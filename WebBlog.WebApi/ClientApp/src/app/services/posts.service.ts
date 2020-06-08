import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PostModel } from '../components/posts/posts.component';


@Injectable({
  providedIn: 'root'
})
export class PostsService {

  private basePostUrl = environment.API_URL +'/api/Posts';
  constructor(private http: HttpClient){}

  getAll(): Observable<any>{
    return this.http.get(this.basePostUrl);
  }

  getPost(id): Observable<PostModel>{
    return this.http.get<PostModel>(this.basePostUrl + "/" + id);
  }
}
