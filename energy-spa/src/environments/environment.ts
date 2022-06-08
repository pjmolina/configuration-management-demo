import { Configuration, version } from './configuration';

export const environment: Configuration = {
  production: false,
  debug: true,
  version,
  serverUri: 'https://localhost:5001',
};
