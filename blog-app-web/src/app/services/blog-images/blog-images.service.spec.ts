import { TestBed } from '@angular/core/testing';

import { BlogImagesService } from './blog-images.service';

describe('BlogImagesService', () => {
  let service: BlogImagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BlogImagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
