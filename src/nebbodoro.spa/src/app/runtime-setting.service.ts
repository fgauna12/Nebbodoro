import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class RuntimeSettingService {

  isInitialized: boolean;
  cachedSettings : IRuntimeSettings;
  constructor( ) { }

  init = () => {
    this.load().then(settings => {
      this.cachedSettings = settings;
      this.isInitialized = true;
    });
  }

  load = (): Promise<IRuntimeSettings> => {
    if (environment.production){
      return new Promise<IRuntimeSettings>((resolve, reject) => {

        var xmlhttp = new XMLHttpRequest(),
          method = 'GET',
          url = '/api/environments';
      
        xmlhttp.open(method, url, true);
      
        xmlhttp.onload = function () {
          if (xmlhttp.status === 200) {
            resolve(<IRuntimeSettings>JSON.parse(xmlhttp.responseText));
          } else {
            throw new Error("Could not obtain runtime settings");
          }
        };
      
        xmlhttp.send();
      });
    }
    else {
      return new Promise<IRuntimeSettings>((resolve, reject) => {
        resolve(environment);
      });
    }
  }
}

interface IRuntimeSettings {
  apiUrl: string;
}
