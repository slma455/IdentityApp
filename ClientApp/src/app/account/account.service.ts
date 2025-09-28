import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Register } from '../shared/models/register';
import { environment } from 'src/environments/environment.development';
import { Login } from '../shared/models/login';
import { User } from '../shared/models/user';
import { of, ReplaySubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  refreshUser(jwt: string | null): Observable<User | undefined> {
    if (jwt === null) {
      this.userSource.next(null);
      return of(undefined);
    }
    
    let headers = new HttpHeaders().set('Authorization', `Bearer ${jwt}`);
    return this.http.get<User>(`${environment.appUrl}/api/account/RefreshUserToken`, { headers }).pipe(
      map((user: User) => {
        this.setUser(user);
        return user;
      })
    );
  }

  login(model: Login) {
    return this.http.post<User>(`${environment.appUrl}/api/account/Login`, model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
        return user;
      })
    );
  }

  logout() {
    localStorage.removeItem(environment.userKey);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
  }

  register(model: Register) {
    return this.http.post(`${environment.appUrl}/api/account/Register`, model);
  }

  getJWT(): string | null {
    const key = localStorage.getItem(environment.userKey);
    if (key) {
      const user: User = JSON.parse(key);
      return user.JWT;
    }
    return null;
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
  }
}