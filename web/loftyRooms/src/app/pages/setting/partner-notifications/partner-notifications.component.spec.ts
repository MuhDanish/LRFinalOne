import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerNotificationsComponent } from './partner-notifications.component';

describe('PartnerNotificationsComponent', () => {
  let component: PartnerNotificationsComponent;
  let fixture: ComponentFixture<PartnerNotificationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PartnerNotificationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
