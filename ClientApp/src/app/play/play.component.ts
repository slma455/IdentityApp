import { Component, OnInit } from '@angular/core';
import { PlayService } from './play.service';

@Component({
  selector: 'app-play',
  templateUrl: './play.component.html',
  styleUrls: ['./play.component.css']
})
export class PlayComponent implements OnInit {
   message:string='';
  constructor(private playService: PlayService) { }

  ngOnInit(): void {
        debugger;

  

  this.playService.getPlayers().subscribe({
  next: (response: any) => {
    console.log('✅ API response:', response);
    this.message = response.value?.message;

  },
  error: (err: any) => {
    console.error('❌ API error:', err);
    this.message = 'An error occurred while fetching players.';
  }
});

  }

}
