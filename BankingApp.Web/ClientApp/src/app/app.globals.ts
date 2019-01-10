import { HttpHeaders } from '@angular/common/http';

//Default headers for authorized users
export function getAuthHttpHeaders() {
  return new HttpHeaders({
    "Authorization": "Bearer " + localStorage.getItem("jwt"),
    "Content-Type": "application/json"
  });
}

