import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Register } from '../shared/models/register';
import { environment } from 'src/environments/environment.development';
import { Login } from '../shared/models/login';
import { User } from '../shared/models/user';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();

  constructor(private http: HttpClient) {}

login(model: Login) {
  return this.http.post<User>(`${environment.appUrl}/api/account/Login`, model).pipe(
    map((user: User) => {
      if (user) {
        this.setUser(user);
      }
      return user; // <-- return user so subscriber gets it
    })
  );
}


  register(model: Register) {
    return this.http.post(`${environment.appUrl}/api/account/Register`, model);
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
    this.user$.subscribe({next:Response =>console.log(Response)})
  }
}
