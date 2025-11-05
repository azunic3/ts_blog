import { TestBed } from '@angular/core/testing';

import { BlogpostsService } from './blogposts.service';

describe('BlogpostsService', () => {
  let service: BlogpostsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BlogpostsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
