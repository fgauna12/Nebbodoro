import { TestBed, inject } from '@angular/core/testing';

import { RuntimeSettingService } from './runtime-setting.service';

describe('RuntimeSettingService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RuntimeSettingService]
    });
  });

  it('should be created', inject([RuntimeSettingService], (service: RuntimeSettingService) => {
    expect(service).toBeTruthy();
  }));
});
