import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlogpostDetailsComponent } from './blogpost-details.component';

describe('BlogpostDetailsComponent', () => {
  let component: BlogpostDetailsComponent;
  let fixture: ComponentFixture<BlogpostDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BlogpostDetailsComponent]
    });
    fixture = TestBed.createComponent(BlogpostDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
