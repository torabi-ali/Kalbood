class AppSettings {
    private static instance: AppSettings

    private _name: string;
    public get name (): string {
      return this._name
    }

    private _title: string;
    public get title (): string {
      return this._title
    }

    private _description: string;
    public get description (): string {
      return this._description
    }

    private _keywords: string;
    public get keywords (): string {
      return this._keywords
    }

    private _author: string;
    public get author (): string {
      return this._author
    }

    private _url: string;
    public get url (): string {
      return this._url
    }

    private _logo: string;
    public get logo (): string {
      return this._logo
    }

    private _phone: string;
    public get phone (): string {
      return this._phone
    }

    private _email: string;
    public get email (): string {
      return this._email
    }

    private _whatsapp: string;
    public get whatsapp (): string {
      return this._whatsapp
    }

    private _telegram: string;
    public get telegram (): string {
      return this._telegram
    }

    private _instagram: string;
    public get instagram (): string {
      return this._instagram
    }

    private _baseApiUrl: string;
    public get baseApiUrl (): string {
      return this._baseApiUrl
    }

    private constructor () {
      this._name = 'کالبود'
      this._title = 'استودیو کالبود'
      this._description = 'معمارانه می‌بینیم، می‌شنویم و احساس می‌کنیم ... برگزاری ورکشاپ با تلفیق معماری با دیگر علوم برای بزرگسالان، نوجوانان و کودکان'
      this._keywords = 'کالبود,استودیو,استودیو کالبود,معماری,سینما,کودک,اینستاگرام,ورکشاپ,ورکشاپ معماری'
      this._author = 'Kalbood'
      this._url = 'https://kalbood.ir/'
      this._logo = '/image/kalbood-logo-original.png'
      this._phone = ''
      this._email = 'info@kalbood.ir'
      this._whatsapp = '#'
      this._telegram = '#'
      this._instagram = 'https://www.instagram.com/kalbood_studio/'
      this._baseApiUrl = 'https://localhost:44366/api/'
    }

    public static getInstance (): AppSettings {
      if (!AppSettings.instance) {
        AppSettings.instance = new AppSettings()
      }

      return AppSettings.instance
    }
}

export default AppSettings
