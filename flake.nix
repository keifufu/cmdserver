{
  description = "stupid fucking fuckserver";
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
  };
  outputs = inputs@{ self, flake-parts, ... }: 
  flake-parts.lib.mkFlake { inherit inputs; } {
    systems = [
      "x86_64-linux"
      "aarch64-linux"
      "x86_64-darwin"
      "aarch64-darwin"
    ];
    perSystem = { pkgs, system, lib, ...}: {
      packages.default = pkgs.buildDotnetModule {
        pname = "cmdserver";
        version = "1.0.0";
        src = ./.;
        projectFile = "cmdserver.csproj";
        meta = {
          license = pkgs.lib.licenses.mit;
          mainProgram = "cmdserver";
        };
      };
    };
  };
}
