import { Component, OnInit } from '@angular/core';
import { PostModel } from '../posts/posts.component';
import { PostsService } from 'src/app/services/posts.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.css']
})
export class PostDetailsComponent implements OnInit {

  private post : PostModel;
  private activeId : number;

  constructor(private service: PostsService, private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activeId = this.activeRoute.snapshot.params['id'];
    this.service.getPost(this.activeId).subscribe((post : PostModel) => {
      this.post = post
    });
  }

}
