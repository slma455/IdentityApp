import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PlayService {

  constructor(private http: HttpClient) { }

  getPlayers() {
    debugger;

    // 🔍 Log the current environment configuration
    console.log('🌎 Environment configuration:', environment);

    // Get the user object from localStorage
    const userJson = localStorage.getItem(environment.userKey);
    console.log('📦 Raw user data from localStorage:', userJson);

    const user = userJson ? JSON.parse(userJson) : null;

    const token = user?.jwt; // extract the JWT
    console.log('🔐 Extracted token from user:', token);

    const url = `${environment.appUrl}/api/Play/Get-Players`;
    console.log('🌍 Request URL:', url);

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    console.log('📤 Request headers:', headers);

    // Make the HTTP call
    return this.http.get(url, { headers });
  }
}
