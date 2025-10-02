import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MassegeComponentComponent } from './massege.component.component';

describe('MassegeComponentComponent', () => {
  let component: MassegeComponentComponent;
  let fixture: ComponentFixture<MassegeComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MassegeComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MassegeComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
