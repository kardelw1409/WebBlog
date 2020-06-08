import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}

export interface PostModel {
  id: number,
  title: string,
  content: string,
  creationTime: Date,
  lastModifiedTime: Date,
  userId: string,
  userName: string,
  categoryId: number,
  categoryName: string,
  postImage: string
}