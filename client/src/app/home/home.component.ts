import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  registerMode = false;
  products: any;

  constructor(private http: HttpClient) {
  }

  ngOnInit(){
    this.getProducts()
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  getProducts() {
    this.http.get('https://localhost:5001/api/Products').subscribe({
      next: response => this.products = response,
      error: err => console.log(err),
      complete: () => console.log('Request has completed')
    });
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }
}
