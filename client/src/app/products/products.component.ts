import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit{
  products: any;
  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.http.get('https://localhost:5001/api/Products').subscribe({
      next: response => this.products = response,
      error: err => console.log(err),
      complete: () => console.log('Request has completed')
    });
  }
}
