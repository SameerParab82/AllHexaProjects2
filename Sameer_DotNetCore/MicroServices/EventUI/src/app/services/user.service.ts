import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly API_URL: string = "http://localhost:61582/api/Identity";
  constructor(private http: HttpClient) { }

  addUser(user: any): Observable<any> {
    let options =
    {
      headers: {
        "Content-Type": "application/json",
        "Accept": "application/json"
      }
    };

    return this.http.post<any>(`${this.API_URL}/auth/register`, user, options);

  }

  getToken(user:any):Observable<any>
  {
    let options =
    {
      headers: {
        "Content-Type": "application/json",
        "Accept": "application/json"
      }
    };

    return this.http.post<any>(`${this.API_URL}/auth/token`, user, options);
  }
}
