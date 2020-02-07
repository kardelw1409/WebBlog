import { Component, OnInit } from '@angular/core';
import { PostsService } from 'src/app/services/posts.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-post-home-items',
  templateUrl: './post-home-items.component.html',
  styleUrls: ['./post-home-items.component.css']
})
export class PostHomeItemsComponent implements OnInit {

  private posts : PostModel[];  

  constructor(private service: PostsService){}

  ngOnInit() {
    this.service.getAll().subscribe((posts : PostModel[]) => {
      this.posts = posts
    });
  }
}

export interface PostModel {
  id: number,
  title: string,
  content: string,
  creationTime: Date,
  lastModifiedTime: Date,
  hasImage: boolean,
  userId: string,
  categoryId: number,
  postImage: string,
  isConfirmed: boolean
}

