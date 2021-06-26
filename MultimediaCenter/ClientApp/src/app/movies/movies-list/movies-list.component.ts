import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Movie, PaginatedMovies } from '../movie.model';
import { MoviesService } from '../movies.service';

@Component({
  selector: 'app-list-movies',
  templateUrl: './movies-list.component.html',
  styleUrls: ['./movies-list.component.css']
})
export class MoviesListComponent{

  public movies: PaginatedMovies;
  currentPage: number;

  constructor(private moviesService: MoviesService) {

  }


  getMovies(page: number = 1) {
    this.currentPage = page;
    this.moviesService.getMovies(page).subscribe(m => this.movies = m);
  }

  ngOnInit() {
    this.getMovies();
  }

}
