import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ImageResponse } from './common';

@Injectable({
  providedIn: 'root'
})
export class GenericServiceService {

  constructor(private http: HttpClient, private matSnackBar: MatSnackBar) {

  }

  private loading: boolean = false;
  private headerName: string = '';
  setLoading(loading: boolean) {
    this.loading = loading;
  }

  getLoading(): boolean {
    return this.loading;
  }

  setHeaderName(name:string){
    this.headerName = name;
  }
  getHeaderName(): string {
    return this.headerName;
  }

  //////////////For get data from api //////////////////////
  get(url: string, data: any): Observable<any> {
    this.setLoading(true);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${environment.GET_TOKEN()}`
    });
    var apiurl = environment.API_URL + url;
    return this.http.get(apiurl, { headers: header }).pipe(catchError(this.errorHandler));
  }
  //////////////For save data api //////////////////////
  post(url: string, data: any): Observable<any> {
    this.setLoading(true);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${environment.GET_TOKEN()}`
    });
    data.CreatedBy = environment.GET_USERID();
    var apiurl = environment.API_URL + url;
    return this.http.post(apiurl, data, { headers: header }).pipe(catchError(this.errorHandler));
  }

  //////////////For get data from api //////////////////////
  getDatatable(url: string, data: any): Observable<any> {
    this.setLoading(true);
    const header = new HttpHeaders({
      'Authorization': `Bearer ${environment.GET_TOKEN() }`
    });
    header.set('Content-Type', 'application/json; charset=utf-8');
    var apiurl = environment.API_URL + url;
   return this.http.post(apiurl, data, { headers: header }).pipe(catchError(this.errorHandler));
  }

  errorHandler(error: HttpErrorResponse) {
    // this.setLoading(false);
    if(error.status == 401)
    {
      window.location.href = '/#/login';
    }
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.log('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.log(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return ('Something bad happened; please try again later.');   
  }

  showSuccessMessage(message: string) {
    this.matSnackBar.open(message, '', {
      duration: 3000,
      panelClass: "success-dialog"
    });
  }
  showErrorMessage(message: string) {
    this.matSnackBar.open(message, '', {
      duration: 3000,
      panelClass: "error-dialog"
    });
  }
  
  IsSaveData(data:any){
    var IsSaveUpdate = false;
    var msg = data.message;
    if(data.statusCode == 11)
    {
      this.showSuccessMessage(msg);
      IsSaveUpdate = true;
    }
    if(data.statusCode == -11)
    {
      this.showErrorMessage(msg);
      IsSaveUpdate = false;
    }
    if(data.statusCode == -12)
    {
      this.showErrorMessage(msg);
      IsSaveUpdate = false;
    }
    return IsSaveUpdate;
  }


  IsNotificationSend(data:any){
    var IsSaveUpdate = false;
    var msg = data.message;
    if(data.result == "success")
    {
      this.showSuccessMessage(msg);
      IsSaveUpdate = true;
    }
    if(data.result == "error")
    {
      this.showErrorMessage(msg);
      IsSaveUpdate = false;
    }

    return IsSaveUpdate;
  }


  IsAuthorized(value:any){
    var claims = environment.GET_CLAIMS();
    var IsValid = false;
    claims!.split(',').map( e => {
      if(e == value) IsValid = true;
    });
    return IsValid;
  }

  uploadFileUsingPath(uploadedFiles: any[],path:string) {
    let file: File | null = null;
    let formData: FormData = new FormData();
    if (uploadedFiles != null && uploadedFiles.length > 0) {
        file = uploadedFiles[0];
    }
    if (file != null) {
        formData.append("img", file, file.name);
        formData.append("path",path);
    } else {
        formData.append("img", '');
    }
    return this.http.post<ImageResponse>(`${environment.API_URL}Candidate/UploadImageUsingPath`, formData);
}
}
