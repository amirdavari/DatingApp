import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent {

  error: any;

  constructor(private router: Router) {
    this.error = router.getCurrentNavigation()?.extras?.state?.['error'];
  }
}
