import { Component, OnInit } from '@angular/core';
import { AppConfig } from '../services/app-config';
import { AppConfigService } from '../services/app-config.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public appConfig?: AppConfig;
  cssUrl = '';

  constructor(private appConfigService: AppConfigService) {}

  ngOnInit(): void {
    this.appConfig = this.appConfigService.getConfig();
    this.cssUrl = `/assets/brands/${this.appConfig?.brand}/site.css`;
  }
}
