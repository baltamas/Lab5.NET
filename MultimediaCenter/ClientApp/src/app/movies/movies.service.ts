import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedMovies } from './PaginatedMovieModel';

@Injectable({
  providedIn: 'root'
})
export class MoviesService {

  private apiUrl: string;

  constructor(private httpClient: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.apiUrl = apiUrl;
  }

  getMovies(page: number): Observable<PaginatedMovies> {
    return this.httpClient.get<PaginatedMovies>(this.apiUrl + 'movies', { params: { 'page': page } });

  }

}


