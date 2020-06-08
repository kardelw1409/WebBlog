import { Component, OnInit } from '@angular/core';
import { PostsService } from 'src/app/services/posts.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-post-home-items',
  templateUrl: './post-home-items.component.html',
  styleUrls: ['./post-home-items.component.css']
})
export class PostHomeItemsComponent implements OnInit {

  private posts : PostHomeModel[];  

  constructor(private service: PostsService){}

  ngOnInit() {
    this.service.getAll().subscribe((posts : PostHomeModel[]) => {
      this.posts = posts
    });
  }
}

export interface PostHomeModel {
  id: number,
  title: string,
  lastModifiedTime: Date,
  userName: string,
  postImage: string,
}

