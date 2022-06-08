import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss'],
})
export class BannerComponent {
  title = '';
  logoUrl?: string;

  private _brand: string | undefined = 'none';

  @Input()
  get brand(): string | undefined {
    return this._brand;
  }
  set brand(br: string | undefined) {
    this._brand = br;

    const normalizedBrand = (br || '').trim().toLowerCase();

    switch (normalizedBrand) {
      case 'electron':
        this.title = 'Electron Energies';
        break;
      case 'photon':
        this.title = 'Photon Bateries Included';

        break;
      default:
        this.title = 'Acme Incorporated';
        break;
    }
    this.logoUrl = normalizedBrand
      ? `/assets/brands/${normalizedBrand}/logo.png`
      : undefined;
  }
}
