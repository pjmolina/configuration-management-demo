export interface AppConfig {
  version: string;
  environment: string;
  audience: string;
  clientId: string;
  origin?: string;
  userAgent?: string;
  acceptedLang?: string;
  langResources?: string;
  lang?: string;
  ip?: string;
  brand: string;
}
