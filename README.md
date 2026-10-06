# Acompanhamento de Consultas — versão PWA

Este pacote é o mesmo app de antes, agora preparado para ser instalado como
aplicativo (PWA) e hospedado em um serviço estático.

## O que foi adicionado

- `manifest.json` — nome, ícones e cores do app para a tela de instalação.
- `sw.js` — service worker: guarda o "casco" do app (HTML/CSS/JS/ícones) em
  cache, então ele abre mesmo sem internet depois da primeira visita.
- `icons/` — ícones em vários tamanhos (incluindo versões "maskable" para
  Android).
- Um botão **"Instalar app"** na barra lateral, que aparece automaticamente
  quando o navegador permite instalação.
- `vercel.json` e `netlify.toml` — cabeçalhos recomendados para o service
  worker funcionar corretamente em cada plataforma.

**Importante:** os dados continuam salvos só no `localStorage` do navegador,
como antes. Isso não muda com o deploy — cada dispositivo/navegador tem seu
próprio armazenamento local, mesmo acessando o mesmo link hospedado. Nada é
enviado para nenhum servidor.

## Testar localmente antes de publicar

PWA exige HTTPS ou `localhost` (não funciona abrindo o arquivo direto com
`file://`). Duas formas simples de testar localmente:

```bash
# Se tiver Python instalado, dentro da pasta do projeto:
python3 -m http.server 8080
# depois abra http://localhost:8080
```

ou, com Node instalado:

```bash
npx serve .
```

## Deploy no Netlify (mais simples)

1. Acesse [app.netlify.com](https://app.netlify.com) e crie uma conta.
2. Na tela inicial, arraste a **pasta inteira** deste projeto para a área de
   upload ("Deploy manually" / "drag and drop").
3. Pronto — o Netlify já publica com HTTPS automaticamente.
4. Para atualizar depois, basta arrastar a pasta novamente.

Alternativa via linha de comando:

```bash
npm install -g netlify-cli
netlify deploy --prod
```

## Deploy no Vercel

1. Acesse [vercel.com](https://vercel.com) e crie uma conta.
2. Instale a CLI e publique direto da pasta:

```bash
npm install -g vercel
vercel --prod
```

3. Ou pelo painel: "Add New Project" → "Deploy" → arraste a pasta. Quando
   perguntar o *framework preset*, escolha **"Other"** (é um site estático,
   sem build).

## Sobre o Lovable

O Lovable é voltado para *gerar* aplicativos a partir de prompts, e nem
sempre aceita importar um projeto estático pronto como este sem adaptação.
Para este app (HTML/CSS/JS puro, sem build), **Netlify ou Vercel são o
caminho mais direto** — publicam a pasta exatamente como está, em poucos
minutos, com HTTPS e PWA funcionando. Se ainda assim quiser usar o Lovable,
o caminho costuma ser abrir um projeto novo lá e colar o conteúdo de
`index.html` como ponto de partida, ajustando o que a plataforma pedir.

## Estrutura de arquivos

```
.
├── index.html          # o app (mesmo conteúdo de antes + tags de PWA)
├── manifest.json        # metadados de instalação
├── sw.js                 # service worker (funcionamento offline)
├── vercel.json           # cabeçalhos para deploy no Vercel
├── netlify.toml          # cabeçalhos para deploy no Netlify
└── icons/
    ├── icon-192.png
    ├── icon-512.png
    ├── icon-maskable-192.png
    ├── icon-maskable-512.png
    ├── apple-touch-icon.png
    ├── favicon-32.png
    └── favicon-64.png
```

## Depois de publicar

- No celular (Android/Chrome ou iOS/Safari), abra o link publicado e use
  "Adicionar à tela inicial" (ou toque no botão "Instalar app" que aparece
  na barra lateral, quando disponível).
- No computador (Chrome/Edge), um ícone de instalação aparece na barra de
  endereço, ou use o botão "Instalar app" na barra lateral.
