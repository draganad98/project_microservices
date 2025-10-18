import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalListComponent } from './global-list.component';

describe('GlobalListComponent', () => {
  let component: GlobalListComponent;
  let fixture: ComponentFixture<GlobalListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GlobalListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GlobalListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
