import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { SharedService } from '../shared.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard  {
  constructor(private accountService:AccountService,private sharedService:SharedService,private router:Router){
    {}

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean >  {
            debugger;

    return this.accountService.user$.pipe(
      map(user=>{
        if(user) return true;
        this.sharedService.showNotification(false,"Unauthorized","You must be logged in to access this page.");
        this.router.navigateByUrl('/account/login');
        return false;
      })
    ) as Observable<boolean>;
  }
  
}
