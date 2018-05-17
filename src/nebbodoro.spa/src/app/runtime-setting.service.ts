import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class RuntimeSettingService {

  constructor( ) { }

  load = ():Promise<any> => {
    return new Promise<any>((resolve, reject) => {

      var xmlhttp = new XMLHttpRequest(),
        method = 'GET',
        url = '/api/environments';
    
      xmlhttp.open(method, url, true);
    
      xmlhttp.onload = function () {
        if (xmlhttp.status === 200) {
          resolve(JSON.parse(xmlhttp.responseText));
        } else {
          resolve(environment);
        }
      };
    
      xmlhttp.send();
    });
  }
}
