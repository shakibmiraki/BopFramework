import { FileService } from '../../../core/services/file.service';
import { Component, OnInit, Input } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  public progress: number;
  public message: string;
  isRequesting: boolean;
  constructor(private fileService: FileService) {}

  @Input()
  language: string;

  @Input()
  isDisabled: boolean;

  ngOnInit(): void {}

  upload(files) {
    this.isRequesting = true;
    if (files.length === 0) {
      this.isRequesting = false;
      return;
    }

    let fileToUpload = files[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.fileService
      .upload(formData, this.language)
      .pipe(finalize(() => (this.isRequesting = false)))
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round((100 * event.loaded) / event.total);
        }
        if (event.type === HttpEventType.Response) {
          console.log(event.body);
        }
      });
  }
}
