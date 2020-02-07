import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { CarouselComponent } from './components/carousel/carousel.component';
import { PostsComponent } from './components/posts/posts.component';
import { AboutComponent } from './components/about/about.component';
import { ContactComponent } from './components/contact/contact.component';
import { RegisterComponent } from './components/register/register.component';
import { PostHomeItemsComponent } from './components/post-home-items/post-home-items.component';
import { PostsService } from './services/posts.service';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    CarouselComponent,
    PostsComponent,
    AboutComponent,
    ContactComponent,
    RegisterComponent,
    PostHomeItemsComponent
  ],
  providers: [PostsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
